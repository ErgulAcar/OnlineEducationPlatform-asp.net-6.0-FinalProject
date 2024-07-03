using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class PuanlamaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PuanlamaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //puanı listeleme ekranım
        public async Task<IActionResult> List()
        {
            //Puanlama
            var client = _httpClientFactory.CreateClient("apiClient");
            var response = await client.GetAsync($"https://localhost:7246/api/Puanlama/");
            var puanlama = await response.Content.ReadFromJsonAsync<List<Puanlama>>();

            //dtodan bir model
            var modelList = new List<PuanlamaListeDTO>();

            foreach (var item in puanlama)
            {
                //tek tek Kursun Idleri ile bilgilere erişip DTOmuza atıyoruz
                var model = new PuanlamaListeDTO();
                model.Id = item.Id;
                var responseKurs = await client.GetAsync($"https://localhost:7246/api/Kurs/{item.KursId}");
                if (responseKurs.IsSuccessStatusCode)
                {
                    var kurs = await responseKurs.Content.ReadFromJsonAsync<Kurs>();
                    model.Baslik = kurs.Baslik;
                    model.ResimUrl = kurs.ResimUrl;
                }
                //tek tek UserIdleri ile bilgilere erişip DTOmuza atıyoruz
                var responseUser = await client.GetAsync($"https://localhost:7246/api/User/MaileGoreUserGet/{item.KullaniciMail}");
                if (responseUser.IsSuccessStatusCode)
                {
                    var user = await responseUser.Content.ReadFromJsonAsync<User>();
                    model.UserMail = user.Mail;
                    model.UserAd = user.Ad;
                    model.UserSoyad = user.Soyad;
                }
                model.Puan = item.Puan;

                modelList.Add(model);
            }


            return View(modelList);

        }


        //detaylistteki yıldızlama ilemi ile verileri getiriyor o gelen verileri kayıt işlemi sağlıyor detaylistte ekranına geri yönlendiriyor
        public async Task<IActionResult> Create(int puan, string kursId)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //Kursu Getiriyoruz
            var responseget = await client.GetAsync($"https://localhost:7246/api/Kurs/{kursId}");
            var kurs = await responseget.Content.ReadFromJsonAsync<KursDTO>();

            //kullanıcı maili alıyoruz
            var mail = HttpContext.Session.GetString("UserMail");


            var model = new PuanlamaDTO()
            {
                KullaniciMail = mail,
                KursId = kurs.Id,
                Puan = puan,
            };
            //apiye ekleme isteği atıyoruz
            var response = await client.PostAsJsonAsync("https://localhost:7246/api/Puanlama", model);
            if (response.IsSuccessStatusCode)
            {
                //Kursun Puanını getiriyoruz 
                var responsegetPuan = await client.GetAsync($"https://localhost:7246/api/Puanlama/KursPuanTablo/{kursId}");
                var kursPuani = await responsegetPuan.Content.ReadFromJsonAsync<double>();
                var puanGuncelleDTO = new PuanUpdateDTO
                {
                    puan = Math.Round(kursPuani, 3),
                    kursId = kursId
                };

                var content = new StringContent(JsonConvert.SerializeObject(puanGuncelleDTO), Encoding.UTF8, "application/json");
                var responsegetPuanPut = await client.PutAsync($"https://localhost:7246/api/Kurs/PuanGuncelle", content);
                if (responsegetPuanPut.IsSuccessStatusCode)
                {
                    TempData["SuccessPuanlamaMessage"] = "Puanlama başarıyla oluşturuldu!";
                }
            }
            else
            {
                TempData["ErrorPuanlamaMessage"] = "Puanlama oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("getKursByDetail", "Kurs", new { Id = kurs.Id });
        }

    }
}
