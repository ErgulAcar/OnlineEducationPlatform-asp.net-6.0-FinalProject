using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class YorumController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public YorumController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //yorumları listeleme ekranım
        public async Task<IActionResult> List()
        {
            //Yorum
            var client = _httpClientFactory.CreateClient("apiClient");
            var responseList = await client.GetAsync($"https://localhost:7246/api/Yorum/");
            var yorum = await responseList.Content.ReadFromJsonAsync<List<Yorum>>();

            var modelYorumGorunumList = new List<YorumGorunumDTO>();
            //modelimizi viewdeki görüntüye ceviriyoruz
            foreach (var item in yorum)
            {
                var response = await client.GetAsync($"https://localhost:7246/api/User/{item.KullaniciId}");
                var user = await response.Content.ReadFromJsonAsync<User>();

                var modelYorumGorunum = new YorumGorunumDTO();
                modelYorumGorunum.Mail = user.Mail;
                modelYorumGorunum.Ad = user.Ad;
                modelYorumGorunum.Soyad = user.Soyad;
                modelYorumGorunum.Icerik = item.Icerik;
                modelYorumGorunum.YorumTarihi = item.YorumTarihi;
                modelYorumGorunumList.Add(modelYorumGorunum);
            }

            return View(yorum);
        }


        //yorum ekleme fonksiyonum
        public async Task<IActionResult> Create(YorumDTO yorumDTO)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //gelen veriler ile eklemek için apıye istekk atıyoruz
            var response = await client.PostAsJsonAsync("https://localhost:7246/api/Yorum", yorumDTO);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessYorumMessage"] = "Yorum başarıyla oluşturuldu!";

            }
            else
            {
                TempData["ErrorYorumMessage"] = "Yorum oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("getKursByDetail", "Kurs", new { Id = yorumDTO.KursId });
        }


        //yorumu silme fonksiyonum
        public async Task<IActionResult> Delete(string Id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //Kursun Idsi lazım bize bu sebeble yorumu Getiriyoruz ve okuyoruz
            var responseYorum = await client.GetAsync($"https://localhost:7246/api/Yorum/{Id}");
            var yorum = await responseYorum.Content.ReadFromJsonAsync<Yorum>();

            //yorumu siliyoruz
            var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Yorum/{Id}");

            if (responsesil.IsSuccessStatusCode)
            {
                TempData["SuccessYorumMessage"] = "Yorum başarıyla silindi!";

            }
            else
            {
                TempData["ErrorYorumMessage"] = "Yorum silinirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("getKursByDetail", "Kurs", new { Id = yorum.KursId });

        }


    }
}
