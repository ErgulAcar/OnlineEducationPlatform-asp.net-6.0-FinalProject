using Core.DTOs;
using Core.IImagesAndIVideosSevices;
using Core.IServices;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using X.PagedList;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class KursController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IImageService _imageService;
        public KursController(IHttpClientFactory httpClientFactory, IImageService imageService)
        {
            _httpClientFactory = httpClientFactory;
            _imageService = imageService;
        }


        //kurs listeleme ekranım
        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //apiye getir isteği atıyor
            var response = await client.GetAsync("https://localhost:7246/api/Kurs/");
            var kursList = await response.Content.ReadFromJsonAsync<List<Kurs>>();
            return View(kursList);


        }


        //Satıın alınan kursları listeleme ekranım
        public async Task<IActionResult> SatınAlmaList()
        {
            //satin alma listesini getiriyoruz
            var client = _httpClientFactory.CreateClient("apiClient");
            //apiye satın alınan kursları getirmesi için istekte bulunuyoruz
            var responseSatinAlinankursList = await client.GetAsync("https://localhost:7246/api/SatinAlinanKurs/");

            var SatinAlinankursList = await responseSatinAlinankursList.Content.ReadFromJsonAsync<List<SatinAlinanKursDTO>>();
            //toplamı sql kayıtsız viewde göstermekk için oluşturuyoruz
            decimal toplam = 0;

            foreach (var item in SatinAlinankursList)
            {
                toplam = (decimal)(toplam + item.KursFiyat);
            }
            TempData["toplam"] = toplam;
            TempData["toplamSatinAlinanKurs"] = SatinAlinankursList.Count;

            return View(SatinAlinankursList);
        }


        //ekleme ekrannım
        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //Kategori
            var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");

            var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();

            var model = new FileKursAndKategori()
            {
                kategoriler = kategori,

            };

            return View(model);
        }


        //ekleme ekranımdaki verileri getiriyor o gelen verileri kayıt işlemi sağlıyor list ekranına geri yönlendiriyor
        [HttpPost]
        public async Task<IActionResult> Create(FileKursAndKategori fileKursAndKategori)
        {
            var rol = HttpContext.Session.GetString("UserRole");
            string? resimUrl = null;

            //resim boş değil ise
            if (fileKursAndKategori.FileKursDTO.ResimUrl != null)
            {
                //_imageService ile resmi ekle projeye bana adını ver
                resimUrl = _imageService.CreatePicture(fileKursAndKategori.FileKursDTO.ResimUrl);
                var model = new Kurs()
                {
                    Id = Guid.NewGuid().ToString(),
                    Baslik = fileKursAndKategori.FileKursDTO.Baslik,
                    Aciklama = fileKursAndKategori.FileKursDTO.Aciklama,
                    ResimUrl = resimUrl,
                    YayinlanmaTarihi = DateTime.Now,
                    HocaAd = fileKursAndKategori.FileKursDTO.HocaAd,
                    HocaSoyad = fileKursAndKategori.FileKursDTO.HocaSoyad,
                    Mail = fileKursAndKategori.FileKursDTO.Mail,
                    Fiyat = fileKursAndKategori.FileKursDTO.Fiyat,
                    Puan = 0,
                    KatilimciSayisi = 0,
                    KategoriAd = fileKursAndKategori.FileKursDTO.KategoriAd
                };

                var client = _httpClientFactory.CreateClient("apiClient");
                //apıye ekleme isteiği
                var response = await client.PostAsJsonAsync("https://localhost:7246/api/Kurs", model);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessKursMessage"] = "Kurs başarıyla oluşturuldu!";

                }
                else
                {
                    TempData["ErrorKursMessage"] = "Kurs oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

                }
                //eğer bu işlemi admın yapmışsa kursları listeleme ekranınna yönlendir
                if (rol == "Admin")
                {
                    return RedirectToAction("List", "Kurs");
                }
                return RedirectToAction("Kurslarim", "User");
            }
            else
            {
                TempData["ErrorKursMessage"] = "Kurs oluşturulurken Resim eklenmedi. Lütfen tekrar deneyin.";

            }
            //eğer bu işlemi admın yapmışsa kursları listeleme ekranınna yönlendir
            if (rol == "Admin")
            {
                return RedirectToAction("List", "Kurs");
            }
            return RedirectToAction("Kurslarim", "User");
        }


        //güncelleme ekranım
        //id = KursId
        public async Task<IActionResult> Update(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //ıd ile kuru getir 
            var response = await client.GetAsync($"https://localhost:7246/api/Kurs/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var kurs = await response.Content.ReadFromJsonAsync<KursDTO>();
                var value = new FileKursDTO
                {
                    Id = Id,
                    Baslik = kurs.Baslik,
                    Aciklama = kurs.Aciklama,
                    ResimUrl = null,
                    YayinlanmaTarihi = kurs.YayinlanmaTarihi,
                    HocaAd = kurs.HocaAd,
                    HocaSoyad = kurs.HocaSoyad,
                    Mail = kurs.Mail,
                    Fiyat = kurs.Fiyat,
                    Puan = kurs.Puan,
                    KatilimciSayisi = kurs.KatilimciSayisi,
                    KategoriAd = kurs.KategoriAd
                };

                //Kategori
                var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");

                var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();
                //önceki verileri getirme amacım önceki bilgileri görmesi
                var model = new FileKursAndKategori()
                {
                    FileKursDTO = value,
                    kategoriler = kategori

                };

                return View(model);
            }
            else
            {
                return View("Error");
            }
        }


        //güncelleme ekranımdan gelen verileri alıp aynı ıd den günelleme işlemini sağlıyoruz güncelleme sonrası listeleme ekranına yönlendiriyoruz
        [HttpPost]
        public async Task<IActionResult> Update(FileKursDTO fileKursDTO)
        {
            var rol = HttpContext.Session.GetString("UserRole");
            KursDTO model = null;
            var client = _httpClientFactory.CreateClient("apiClient");
            //ıdye göre kursu getir ve oku
            var responseget = await client.GetAsync($"https://localhost:7246/api/Kurs/{fileKursDTO.Id}");
            var kurs = await responseget.Content.ReadFromJsonAsync<KursDTO>();

            //eğer resim yüklenmişse yeni resim ile apıye istek atıyorum
            if (fileKursDTO.ResimUrl != null)
            {
                var resimUrl = _imageService.UpdatePicture(kurs.ResimUrl, fileKursDTO.ResimUrl);
                model = new KursDTO()
                {
                    Id = fileKursDTO.Id,
                    Baslik = fileKursDTO.Baslik,
                    Aciklama = fileKursDTO.Aciklama,
                    ResimUrl = resimUrl,
                    YayinlanmaTarihi = fileKursDTO.YayinlanmaTarihi,
                    HocaAd = fileKursDTO.HocaAd,
                    HocaSoyad = fileKursDTO.HocaSoyad,
                    Mail = fileKursDTO.Mail,
                    Fiyat = fileKursDTO.Fiyat,
                    Puan = fileKursDTO.Puan,
                    KatilimciSayisi = fileKursDTO.KatilimciSayisi,
                    KategoriAd = fileKursDTO.KategoriAd
                };
            }
            
            //eğer resim yüklenmemişse eski resimi bırakıyorum  ve  apıye istek atıyorum
            else
            {
                model = new KursDTO()
                {
                    Id = fileKursDTO.Id,
                    Baslik = fileKursDTO.Baslik,
                    Aciklama = fileKursDTO.Aciklama,
                    ResimUrl = kurs.ResimUrl,
                    YayinlanmaTarihi = fileKursDTO.YayinlanmaTarihi,
                    HocaAd = fileKursDTO.HocaAd,
                    HocaSoyad = fileKursDTO.HocaSoyad,
                    Mail = fileKursDTO.Mail,
                    Fiyat = fileKursDTO.Fiyat,
                    Puan = fileKursDTO.Puan,
                    KatilimciSayisi = fileKursDTO.KatilimciSayisi,
                    KategoriAd = fileKursDTO.KategoriAd
                };

            }
            var response = await client.PutAsJsonAsync("https://localhost:7246/api/Kurs", model);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessKursMessage"] = "Kurs başarıyla Güncellendi!";

            }
            else
            {
                TempData["ErrorKursMessage"] = "Kurs Güncellenirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            //eğer bu işlemi admın yapmışsa kursları listeleme ekranınna yönlendir
            if (rol == "Admin")
            {
                return RedirectToAction("List", "Kurs");
            }
            return RedirectToAction("Kurslarim", "User");
        }


        //sile batığında gerçekleşen fonksiyonum. silme sonrası listeleme ekranına yönlendiriyoruz
        //ıd = KursId
        public async Task<IActionResult> Delete(string Id)
        {
            var rol = HttpContext.Session.GetString("UserRole");
            var client = _httpClientFactory.CreateClient("apiClient");

            // Sonra kurs altında bolum var mı diye bakıyoruz
            var kursResponse = await client.GetAsync($"https://localhost:7246/api/Bolum/KursVarMi/{Id}");
            var result = await kursResponse.Content.ReadFromJsonAsync<bool>();

            if (result)
            {
                TempData["ErrorKursMessage"] = "Bu Kurs altında Bolum bulunmaktadır. Silme işlemi yapılamaz.";

                //eğer bu işlemi admın yapmışsa kursları listeleme ekranınna yönlendir
                if (rol == "Admin")
                {
                    return RedirectToAction("List", "Kurs");
                }
                return RedirectToAction("Kurslarim", "User");
            }

            //kursu getir
            var responseget = await client.GetAsync($"https://localhost:7246/api/Kurs/{Id}");
            var kurs = await responseget.Content.ReadFromJsonAsync<KursDTO>();

            //resmi siliyoruz
            _imageService.DeletePicture(kurs.ResimUrl);

            //puanlamayı siliyoruz
            var responsePuanSil = await client.DeleteAsync($"https://localhost:7246/api/Puanlama/KursaGoreDeletePuan/{Id}");


            //yorumları siliyoruz
            var responseYorumSil = await client.DeleteAsync($"https://localhost:7246/api/Yorum/KursaGoreDeleteYorum/{Id}");


            //kursu siliyoruz
            var responseSil = await client.DeleteAsync($"https://localhost:7246/api/Kurs/{Id}");


            if (responseSil.IsSuccessStatusCode)
            {
                TempData["SuccessKursMessage"] = "Kurs başarıyla Silindi!";

            }
            else
            {
                TempData["ErrorKursMessage"] = "Kurs Silinirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            //eğer bu işlemi admın yapmışsa kursları listeleme ekranınna yönlendir
            if (rol == "Admin")
            {
                return RedirectToAction("List", "Kurs");
            }
            return RedirectToAction("Kurslarim", "User");

        }


        //kursun detayları ve yorumlar ekranı 
        public async Task<IActionResult> getKursByDetail(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //Kursu Getiriyoruz
            var responseget = await client.GetAsync($"https://localhost:7246/api/Kurs/{Id}");
            var kurs = await responseget.Content.ReadFromJsonAsync<KursDTO>();

            //kullanıcı kursun sahibimi bakıyoruz
            var mail = HttpContext.Session.GetString("UserMail");
            var valueKursOgretmeniMi = HttpContext.Session.GetString("KursOgretmeniMi");

            //giriş yapmış biri ise kontrol sağlansın
            if (valueKursOgretmeniMi != "false" || valueKursOgretmeniMi != "true")
            {
                HttpContext.Session.SetString("KursOgretmeniMi", "false");

                //kursun sağibimi diye bakıyoruz öyle ise KursOgretmeniMi güncelliyoruuz 
                if (kurs.Mail == mail)
                {
                    HttpContext.Session.SetString("KursOgretmeniMi", "true");
                }
            }


            //Kategori
            var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");
            var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();

            //Kursun Yorumunu Getiriyoruz
            var responseYorum = await client.GetAsync($"https://localhost:7246/api/Yorum/kursId/{kurs.Id}");
            var Yorumlar = await responseYorum.Content.ReadFromJsonAsync<List<Yorum>>();

            var modelYorumGorunumList = new List<YorumGorunumDTO>();
            //yorumda gerekli bilgiler yer almıyyoru sadece ıd ler ve içerik tarihi bide içerik yer alıyor görünümmü zenginleştiriyoruz
            foreach (var item in Yorumlar)
            {
                var response = await client.GetAsync($"https://localhost:7246/api/User/{item.KullaniciId}");
                var user = await response.Content.ReadFromJsonAsync<User>();

                var modelYorumGorunum = new YorumGorunumDTO();
                modelYorumGorunum.Id = item.Id;
                modelYorumGorunum.Mail = user.Mail;
                modelYorumGorunum.Ad = user.Ad;
                modelYorumGorunum.Soyad = user.Soyad;
                modelYorumGorunum.Icerik = item.Icerik;
                modelYorumGorunum.YorumTarihi = item.YorumTarihi;
                modelYorumGorunum.ErisimTipi = false;
                if (user.Mail == mail || mail == "Admin")
                {
                    modelYorumGorunum.ErisimTipi = true;
                }
                modelYorumGorunumList.Add(modelYorumGorunum);
            }


            //viewe gerekli model listi oluşturuyoruz
            YorumlarAndKursAndKategoriler modelList = new YorumlarAndKursAndKategoriler()
            {
                kurslar = kurs,
                kategoriler = kategori,
                yorumlar = modelYorumGorunumList
            };

            //Önceden bu kursu almışmı ona bakıyoruz
            var userId = HttpContext.Session.GetString("UserId");

            var valueKursuAlmisMi = HttpContext.Session.GetString("KursuAlmisMi");
            if (valueKursuAlmisMi != "false" || valueKursuAlmisMi != "true")
            {

                HttpContext.Session.SetString("KursuAlmisMi", "false");
                if (userId != null)
                {
                    var responseSatinAlmismi = await client.GetAsync($"https://localhost:7246/api/SatinAlinanKurs/KayitSorgu?userId={userId}&kursId={kurs.Id}");
                    if (responseSatinAlmismi.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContext.Session.SetString("KursuAlmisMi", "true");

                    }
                }
            }


            //kursa önceden puan vermişmi ona bakıyoruz

            var valuePuanVermisMi = HttpContext.Session.GetString("PuanVermisMi");
            if (valuePuanVermisMi != "false" || valuePuanVermisMi != "true")
            {
                HttpContext.Session.SetString("PuanVermisMi", "true");
                if (userId != null)
                {
                    var responsePuanVermisMi = await client.GetAsync($"https://localhost:7246/api/Puanlama/PuanVermisMi?userMail={mail}&kursId={kurs.Id}");
                    if (responsePuanVermisMi.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContext.Session.SetString("PuanVermisMi", "false");
                    }
                }
            }

            return View(modelList);
        }


        //kategoriye göre arama yaparsa ona göre kategori adına göere listeliyor
        public async Task<IActionResult> KategoriyeGoreKurslarList(string kategoriAd)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            var responseget = await client.GetAsync($"https://localhost:7246/api/Kurs/KategoriyeGoreKursList/{kategoriAd}");

            var kurs = await responseget.Content.ReadFromJsonAsync<List<Kurs>>();

            //Kategori
            var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");

            var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();

            ListKurslarAndListKategoriler kurslarAndKategoriler = new ListKurslarAndListKategoriler()
            {
                kurslar = kurs,
                kategoriler = kategori,
                kategori = kategoriAd

            };

            return View(kurslarAndKategoriler);
        }
        

        //kullanıcın girdiği veriye yakın olan kurs adı yada hoca adı,soyadı olan kursları getiriyor
        public async Task<IActionResult> Arama(string girilenText)
        {
            if (girilenText == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var client = _httpClientFactory.CreateClient("apiClient");

            var responseget = await client.GetAsync($"https://localhost:7246/api/Kurs/Arama/{girilenText}");

            var kurs = await responseget.Content.ReadFromJsonAsync<List<Kurs>>();

            //Kategori
            var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");

            var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();

            ListKurslarAndListKategoriler kurslarAndKategoriler = new ListKurslarAndListKategoriler()
            {
                kurslar = kurs,
                kategoriler = kategori
            };

            return View(kurslarAndKategoriler);
        }


    }
}
