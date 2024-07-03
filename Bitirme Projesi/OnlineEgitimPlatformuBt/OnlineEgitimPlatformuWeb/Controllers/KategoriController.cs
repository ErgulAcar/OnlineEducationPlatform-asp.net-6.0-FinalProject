using Core.DTOs;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class KategoriController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public KategoriController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //bolumleri listeleme ekranım
        public async Task<IActionResult> List()
        {
            //Kategori
            var client = _httpClientFactory.CreateClient("apiClient");
            //apiye getir isteği atıyoruz
            var response = await client.GetAsync($"https://localhost:7246/api/Kategori/");
            var kategori = await response.Content.ReadFromJsonAsync<List<Kategori>>();

            return View(kategori);
        }


        //ekleme ekrannım
        public IActionResult Create()
        {
            return View();
        }


        //ekleme ekranımdaki verileri getiriyor o gelen verileri kayıt işlemi sağlıyor list ekranına geri yönlendiriyor
        [HttpPost]
        public async Task<IActionResult> Create(KategoriDTO kategoriDTO)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //apiye verdiğimiz bilgileri ekleme isteği atıyoruz
            var response = await client.PostAsJsonAsync("https://localhost:7246/api/Kategori", kategoriDTO);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessKategoriMessage"] = "Kategori başarıyla oluşturuldu!";

            }
            else
            {
                TempData["ErrorKategoriMessage"] = "Kategori oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Kategori");
        }


        //güncelleme ekranım
        //id = kategoriID
        public async Task<IActionResult> Update(string id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //ıdye ggöre getir isteği
            var response = await client.GetAsync($"https://localhost:7246/api/Kategori/{id}");
            if (response.IsSuccessStatusCode)
            {
                var kategori = await response.Content.ReadFromJsonAsync<Kategori>();
                return View(kategori);
            }
            else
            {
                return View("Error");
            }
        }


        //güncelleme ekranımdan gelen verileri alıp aynı ıd den günelleme işlemini sağlıyoruz güncelleme sonrası listeleme ekranına yönlendiriyoruz
        [HttpPost]
        public async Task<IActionResult> Update(Kategori kategori)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //verilen bilgilerle aynı ıd üzeriinde güncelleme isteğği
            var response = await client.PutAsJsonAsync("https://localhost:7246/api/Kategori", kategori);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessKategoriMessage"] = "Kategori başarıyla Güncellendi!";

            }
            else
            {
                TempData["ErrorKategoriMessage"] = "Kategori Güncellerken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Kategori");
        }


        //sile batığında gerçekleşen fonksiyonum. silme sonrası listeleme ekranına yönlendiriyoruz
        //ıd = kategoriId
        public async Task<IActionResult> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            // Önce kategoriyi getiriyoruz
            var response = await client.GetAsync($"https://localhost:7246/api/Kategori/{id}");
            var kategori = await response.Content.ReadFromJsonAsync<Kategori>();

            // Sonra kategori altında kurs var mı diye bakıyoruz
            var kursResponse = await client.GetAsync($"https://localhost:7246/api/Kurs/KategoriVarMi/{kategori.Ad}");
            var result = await kursResponse.Content.ReadFromJsonAsync<bool>();

            if (result)
            {
                TempData["ErrorKategoriMessage"] = "Bu kategori altında kurslar bulunmaktadır. Silme işlemi yapılamaz.";

                return RedirectToAction("List", "Kategori");
            }

            var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Kategori/{id}");


            if (responsesil.IsSuccessStatusCode)
            {
                TempData["SuccessKategoriMessage"] = "Kategori başarıyla silindi!";

            }
            else
            {
                TempData["ErrorKategoriMessage"] = "Kategori silinirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "Kategori");

        }


    }
}
