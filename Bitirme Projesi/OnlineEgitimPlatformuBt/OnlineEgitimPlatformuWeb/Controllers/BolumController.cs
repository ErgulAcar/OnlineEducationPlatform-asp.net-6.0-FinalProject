using Core.DTOs;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class BolumController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BolumController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //bolumleri listeleme ekranım
        //Id= KursId
        public async Task<IActionResult> List(string Id)
        {
            //Kursu bul
            var client = _httpClientFactory.CreateClient("apiClient");
            var responseKurs = await client.GetAsync($"https://localhost:7246/api/Kurs/{Id}");
            var kurs = await responseKurs.Content.ReadFromJsonAsync<KursDTO>();

            //Kategori getir
            var responseKategori = await client.GetAsync($"https://localhost:7246/api/Kategori/");
            var kategori = await responseKategori.Content.ReadFromJsonAsync<List<Kategori>>();
            

            //Bolum getir
            var response = await client.GetAsync($"https://localhost:7246/api/Bolum/KursGetByBolum/{Id}");
            var bolum = await response.Content.ReadFromJsonAsync<List<Bolum>>();

            //bunu list model olarak viewe yoluluyoruz
            var model = new BolumAndKategoriAndKurs()
            {
                bolumler = bolum,
                kurslar = kurs,
                kategoriler = kategori

            };


            return View(model);
        }


        //kurs ıdye göre ekleme ekrannım
        //Id= KursId
        public async Task<IActionResult> Create(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //Bolum
            var response = await client.GetAsync($"https://localhost:7246/api/Bolum/KursGetByBolum/{Id}");
            var bolum = await response.Content.ReadFromJsonAsync<List<BolumDTO>>();

            int newSiraNo = 1;

            if (bolum.Any())
            {
                newSiraNo = bolum.Max(b => b.SıraNo.GetValueOrDefault()) + 1;
            }

            var model = new BolumDTO
            {
                KursId = Id,
                SıraNo = newSiraNo
            };

            return View(model);

        }


        //ekleme ekranımdaki verileri getiriyor o gelen verileri kayıt işlemi sağlıyor list ekranına geri yönlendiriyor
        [HttpPost]
        public async Task<IActionResult> Create(BolumDTO bolumDTO)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //apiye verdiğimiz bilgileri ekleme isteği atıyoruz
            var response = await client.PostAsJsonAsync("https://localhost:7246/api/Bolum", bolumDTO);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessBolumMessage"] = "Bolum başarıyla oluşturuldu!";

            }
            else
            {
                TempData["ErrorBolumMessage"] = "Bolum oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Bolum", new { Id = bolumDTO.KursId });
        }


        //güncelleme ekranım
        //id = bolumId
        public async Task<IActionResult> Update(string id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //apıden getirme isteğinde bulunuyoruz ıd ile
            var response = await client.GetAsync($"https://localhost:7246/api/Bolum/{id}");
            if (response.IsSuccessStatusCode)
            {
                var bolum = await response.Content.ReadFromJsonAsync<Bolum>();
                return View(bolum);
            }
            else
            {
                return View("Error");
            }
        }
        

        //güncelleme ekranımdan gelen verileri alıp aynı ıd den günelleme işlemini sağlıyoruz güncelleme sonrası listeleme ekranına yönlendiriyoruz
        [HttpPost]
        public async Task<IActionResult> Update(Bolum bolum)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //apıden güncelleme isteği yolluyoruz
            var response = await client.PutAsJsonAsync("https://localhost:7246/api/Bolum", bolum);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessBolumMessage"] = "Bolum başarıyla Güncellendi!";

            }
            else
            {
                TempData["ErrorBolumMessage"] = "Bolum Güncellerken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Bolum", new { Id = bolum.KursId });
        }


        //sile batığında gerçekleşen fonksiyonum. silme sonrası listeleme ekranına yönlendiriyoruz
        //ıd = bolumId
        public async Task<IActionResult> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //bolumü getiriyoruz
            var responsekurs = await client.GetAsync($"https://localhost:7246/api/Bolum/{id}");
            var bolumRead = await responsekurs.Content.ReadFromJsonAsync<Bolum>();

            // Sonra bolum altında ders var mı diye bakıyoruz
            var kursResponse = await client.GetAsync($"https://localhost:7246/api/Ders/BolumVarMi/{id}");
            var result = await kursResponse.Content.ReadFromJsonAsync<bool>();
            
            // ders var ise silme işlemini gerçekleştirmiyoruz hata mesajı yönlendiriyoruz
            if (result)
            {
                TempData["ErrorBolumMessage"] = "Bu Kurs altında Bolum bulunmaktadır. Silme işlemi yapılamaz.";

                return RedirectToAction("List", "Bolum", new { Id = bolumRead.KursId });
            }

            //yok ise silme işlemini gerçekleştiriyoruz
            var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Bolum/{id}");

            if (responsesil.IsSuccessStatusCode)
            {
                TempData["SuccessBolumMessage"] = "Bolum başarıyla Silindi!";

            }
            else
            {
                TempData["ErrorBolumMessage"] = "Bolum Silinirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Bolum", new { Id = bolumRead.KursId });
        }


    }
}
