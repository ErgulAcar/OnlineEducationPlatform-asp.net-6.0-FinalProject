using Core.DTOs;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class SepetController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SepetController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //sepet listeleme ekranım
        public async Task<IActionResult> List()
        {
            //Kategori
            var client = _httpClientFactory.CreateClient("apiClient");
            var responsekategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");
            var kategori = await responsekategori.Content.ReadFromJsonAsync<List<Kategori>>();

            //Sepet
            var response = await client.GetAsync($"https://localhost:7246/api/Sepet/");
            var sepet = await response.Content.ReadFromJsonAsync<List<Sepet>>();
            decimal toplam = 0;
            foreach (var item in sepet)
            {
                toplam = (decimal)(toplam + (item.Fiyat * item.Adet));
            }

            var model = new SepetListAndKategoriList()
            {
                kategoriler = kategori,
                sepetler = sepet,
                ToplamFiyat = toplam
            };
            return View(model);
        }


        //ekleme işlemlerimi gerçekkleştiren fonksiyonum
        //ıd = KursId
        public async Task<IActionResult> Add(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //kursu buluyoruz
            var responseKurs = await client.GetAsync($"https://localhost:7246/api/Kurs/{Id}");
            var kurs = await responseKurs.Content.ReadFromJsonAsync<KursDTO>();

            Sepet sepet = new Sepet()
            {
                Adet = 1,
                Fiyat = kurs.Fiyat,
                Baslik = kurs.Baslik,
                ResimUrl = kurs.ResimUrl,
                Id = Id
            };

            //sepete ekleme isteğim
            var response = await client.PostAsJsonAsync("https://localhost:7246/api/Sepet/", sepet);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["ErrorSepetMessage"] = "Sepete eklenmedi. Ürün mevcut.";
                return RedirectToAction("List", "Sepet", new {Id = sepet.Id});
            }
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessSepetMessage"] = "Sepete eklendi!";

            }
            else
            {
                TempData["ErrorSepetMessage"] = "Sepete eklenmedi. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Sepet");
        }


        //sile batığında gerçekleşen fonksiyonum. silme sonrası listeleme ekranına yönlendiriyoruz
        //ıd = KursId
        public async Task<IActionResult> Delate(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/{Id}");
            if (responsesil.IsSuccessStatusCode)
            {
                TempData["SuccessSepetMessage"] = "Ürün Sepetten Silindi!";

            }
            else
            {
                TempData["ErrorSepetMessage"] = "Ürün Sepetten Silinmedi. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Sepet");
        }


        //satın ala batığında gercekleştirilen işlemlerim
        public async Task<IActionResult> SatinAl(string userId)
        {
            int sayac = 0;

            var client = _httpClientFactory.CreateClient("apiClient");
            DateTime now = DateTime.Now;
            //Sepet
            var response = await client.GetAsync($"https://localhost:7246/api/Sepet/");
            var sepet = await response.Content.ReadFromJsonAsync<List<Sepet>>();

            if (sepet.Count == 0)
            {
                TempData["ErrorSepetMessage"] = "Sepet Boş.";
                return RedirectToAction("List", "Sepet");
            }
            if (userId == null)
            {
                TempData["ErrorSatinAl"] = "Satin alma yapmanız için önce giriş yapmanız lazım. Lütfen tekrar deneyin.";
                TempData["ErrorSatinAlKayit"] = "Veya Satin alma yapmanız için önce Kayit yapmanız lazım. Lütfen tekrar deneyin.";
                return RedirectToAction("LoginAndCreate", "Home");
            }
            //foreach ile ürünleri tek tek ekliyoruz
            foreach (var item in sepet)
            {
                var model = new SatinAlinanKursDTO();
                //kursu getiriyoruz
                var responseKurs = await client.GetAsync($"https://localhost:7246/api/Kurs/{item.Id}");
                //ok döner ise
                if (responseKurs.IsSuccessStatusCode)
                {
                    //okuma işlemi yapıyoruz
                    var kurs = await responseKurs.Content.ReadFromJsonAsync<Kurs>();
                    //boş verilerimizi dolduruyoruz
                    model.KursId = kurs.Id;
                    model.Baslik = kurs.Baslik;
                    model.ResimUrl = kurs.ResimUrl;
                    model.KursFiyat = kurs.Fiyat;
                }
                model.UserId = HttpContext.Session.GetString("UserId");
                model.UserMail = HttpContext.Session.GetString("UserMail");
                model.UserAd = HttpContext.Session.GetString("UserAd");
                model.UserSoyad = HttpContext.Session.GetString("UserSoyad");
                model.SatinAlinmaTarihi = now;

                //ekleme işlemi için apıye modelimizi atıp isket atıyoruz
                var responseAdd = await client.PostAsJsonAsync("https://localhost:7246/api/SatinAlinanKurs/", model);

                //kurusun katılımcı sayısını artıırsın diye istek atıyoruz
                var KatilimciArtir = await client.PutAsJsonAsync("https://localhost:7246/api/Kurs/Katilimci", model);

                if (responseAdd.IsSuccessStatusCode)
                {
                    //istek sayısını tutuyoruz
                    sayac++;
                }
            }
            //eğer istek sayısı ile sepet adeti tutuyprsa tüm ücünler alındığını kullanıcıya mesaj ile iletiyoruz
            if (sepet.Count == sayac)
            {
                TempData["SatinAlmaOlumlu"] = "Ürünler Satin Alındı!";
            }
            //sepeti boşaltmak için istek atıyoruz
            var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
            return RedirectToAction("OgrenimIceriklerim", "User");
        }
    }
}
