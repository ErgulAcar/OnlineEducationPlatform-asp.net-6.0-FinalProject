using Core.DTOs;
using Core.IImagesAndIVideosSevices;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class DersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IVideoSevices _videoSevices;
        public DersController(IHttpClientFactory httpClientFactory, IVideoSevices videoSevices)
        {
            _httpClientFactory = httpClientFactory;
            _videoSevices = videoSevices;
        }


        //dersleri listeleme ekranım
        public async Task<IActionResult> List(string bolumId, string dersId)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //Kategori
            var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");
            var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();

            //bolume göre dersleri listele
            var responsedersler = await client.GetAsync($"https://localhost:7246/api/Ders/BolumGetByDersler/{bolumId}");
            var dersList = await responsedersler.Content.ReadFromJsonAsync<List<Ders>>();

            //burda dersId boş ise boş ekran getirmek istiyorum ama var ise o dersin videosuunu getirmek istiyorum
            DersDTO ders = null;
            if (dersId != null)
            {
                // dersi listele
                var responseders = await client.GetAsync($"https://localhost:7246/api/Ders/{dersId}");
                ders = await responseders.Content.ReadFromJsonAsync<DersDTO>();
            }

            //Bolum
            var response = await client.GetAsync($"https://localhost:7246/api/Bolum/{bolumId}");
            var bolum = await response.Content.ReadFromJsonAsync<BolumDTO>();

            //viewe modelimmi oluşturup yolluyoryum
            var model = new DersAndDerslerAndKategori()
            {
                kategoriler = kategori,
                dersler = dersList,
                ders = ders,
                Bolum = bolum,

            };

            return View(model);
        }


        //BolumIdye göre ekleme ekrannım
        //Id = BolumId
        public async Task<IActionResult> Create(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //Bolum
            var response = await client.GetAsync($"https://localhost:7246/api/Ders/BolumGetByDersler/{Id}");
            var dersList = await response.Content.ReadFromJsonAsync<List<Ders>>();

            int newSiraNo = 1;

            if (dersList.Any())
            {
                newSiraNo = dersList.Max(b => b.SıraNo.GetValueOrDefault()) + 1;
            }

            var model = new FileDersDTO
            {
                BolumId = Id,
                SıraNo = newSiraNo
            };

            return View(model);
        }


        //ekleme ekranımdaki verileri getiriyor o gelen verileri kayıt işlemi sağlıyor list ekranına geri yönlendiriyor
        [HttpPost]
        public async Task<IActionResult> Create(FileDersDTO fileDersDTO)
        {
            string? videoUrl = null;

            if (fileDersDTO.VideoUrl != null)
            {
                //servise verdiğimiz bilgiler ile videoyu eklemesini istiyoruz ve bize dosya ismini veriyor
                videoUrl = _videoSevices.CreateVideo(fileDersDTO.VideoUrl);
                var model = new Ders()
                {
                    Id = Guid.NewGuid().ToString(),
                    Baslik = fileDersDTO.Baslik,
                    Icerik = fileDersDTO.Icerik,
                    VideoUrl = videoUrl,
                    SıraNo = fileDersDTO.SıraNo,
                    BolumId = fileDersDTO.BolumId
                };
                //apiye verdiğimiz bilgileri ekleme isteği atıyoruz
                var client = _httpClientFactory.CreateClient("apiClient");
                var response = await client.PostAsJsonAsync("https://localhost:7246/api/Ders", model);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessDersMessage"] = "Ders başarıyla oluşturuldu!";

                }
                else
                {
                    TempData["ErrorDersMessage"] = "Ders oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

                }
                return RedirectToAction("List", "Ders", new { bolumId = fileDersDTO.BolumId, dersId = model.Id });
            }
            else
            {
                TempData["ErrorDersMessage"] = "Ders oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Ders", new { bolumId = fileDersDTO.BolumId } );
        }


        //güncelleme ekranım
        //id = dersId
        public async Task<IActionResult> Update(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //apiye ıd ile getir için istek atıyoruz 
            var response = await client.GetAsync($"https://localhost:7246/api/Ders/{Id}");
            //response 200 ok dönüyorsa oku verileri valudan view yolla iişlemini gerçekleştiriyouz
            if (response.IsSuccessStatusCode)
            {
                var ders = await response.Content.ReadFromJsonAsync<DersDTO>();
                var value = new FileDersDTO
                {
                    Id = Id,
                    Baslik = ders.Baslik,
                    Icerik = ders.Icerik,
                    VideoUrl = null,
                    SıraNo = ders.SıraNo,
                    BolumId = ders.BolumId
                };

                return View(value);
            }
            else
            {
                return View("Error");
            }
        }


        //güncelleme ekranımdan gelen verileri alıp aynı ıd den günelleme işlemini sağlıyoruz güncelleme sonrası listeleme ekranına yönlendiriyoruz
        [HttpPost]
        public async Task<IActionResult> Update(FileDersDTO fileDersDTO)
        {
            DersDTO model = null;
            var client = _httpClientFactory.CreateClient("apiClient");

            //apiye idye göre getir isteği atıyorum ve okuyorum
            var responseget = await client.GetAsync($"https://localhost:7246/api/Ders/{fileDersDTO.Id}");
            var ders = await responseget.Content.ReadFromJsonAsync<DersDTO>();

            //eğer video yüklenmişse yeni videoyu ile apıye istek atıyorum
            if (fileDersDTO.VideoUrl != null)
            {
                var videoUrl = _videoSevices.UpdateVideo(ders.VideoUrl, fileDersDTO.VideoUrl);
                model = new DersDTO()
                {
                    Id = fileDersDTO.Id,
                    Baslik = fileDersDTO.Baslik,
                    Icerik = fileDersDTO.Icerik,
                    VideoUrl = videoUrl,
                    SıraNo = fileDersDTO.SıraNo,
                    BolumId = fileDersDTO.BolumId
                };
            }

            //eğer video yüklenmemişse eski videoyu bırakıyorum  ile apıye istek atıyorum
            else
            {
                model = new DersDTO()
                {
                    Id = fileDersDTO.Id,
                    Baslik = fileDersDTO.Baslik,
                    Icerik = fileDersDTO.Icerik,
                    VideoUrl = ders.VideoUrl,
                    SıraNo = fileDersDTO.SıraNo,
                    BolumId = fileDersDTO.BolumId
                };

            }
            var response = await client.PutAsJsonAsync("https://localhost:7246/api/Ders", model);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessDersMessage"] = "Ders başarıyla güncellendi!";

            }
            else
            {
                TempData["ErrorDersMessage"] = "Ders güncellenirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Ders", new { bolumId = fileDersDTO.BolumId, dersId = fileDersDTO.Id });
        }


        //sile batığında gerçekleşen fonksiyonum. silme sonrası listeleme ekranına yönlendiriyoruz
        //ıd = dersId
        public async Task<IActionResult> Delete(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //dersi bul
            var responseget = await client.GetAsync($"https://localhost:7246/api/Ders/{Id}");

            var ders = await responseget.Content.ReadFromJsonAsync<DersDTO>();

            var bolumId=ders.BolumId;

            _videoSevices.DeleteVideo(ders.VideoUrl);

            var response = await client.DeleteAsync($"https://localhost:7246/api/Ders/{Id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessDersMessage"] = "Ders başarıyla silindi!";

            }
            else
            {
                TempData["ErrorDersMessage"] = "Ders silinirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Ders", new { BolumId = bolumId });
        }



    }
}
