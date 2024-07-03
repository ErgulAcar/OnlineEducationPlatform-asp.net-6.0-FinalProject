using Core.DTOs;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //kullanıcıları listeleme ekranım
        public async Task<IActionResult> List()
        {
            //User
            var client = _httpClientFactory.CreateClient("apiClient");
            var response = await client.GetAsync($"https://localhost:7246/api/User/UserAndRole");
            var userAndRoleList = await response.Content.ReadFromJsonAsync<List<HomeUserAndRoleDTO>>();


            return View(userAndRoleList);
        }


        ////ekleme ekranım
        public IActionResult Create()
        {

            return View();
        }


        //kulanıcı ekleme ekranımdan gelen verileri yollayıp kaydetme işlemimi yapan fonksiyon
        [HttpPost]
        public async Task<IActionResult> Create(UserDTO userDTO)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            var response = await client.PostAsJsonAsync("https://localhost:7246/api/User", userDTO);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUserMessage"] = "User başarıyla oluşturuldu!";

            }
            else
            {
                TempData["ErrorUserMessage"] = "User oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "User");
        }


        //kullanıcı güncelleme ekranım
        //ıd = UserId
        public async Task<IActionResult> Update(string id)
        {
            // HomeUserAndRoleDTO bilgilerini bulma
            var client = _httpClientFactory.CreateClient("apiClient");
            var response = await client.GetAsync($"https://localhost:7246/api/User/UserAndRole/{id}");
            var user = await response.Content.ReadFromJsonAsync<HomeUserAndRoleDTO>();


            //Rolelist
            var responserole = await client.GetAsync($"https://localhost:7246/api/Role/");
            var role = await responserole.Content.ReadFromJsonAsync<List<Role>>();

            //admin rolünü çıkartıyorum
            role = role.Where(r => r.Ad != "Admin").ToList();


            if (response.IsSuccessStatusCode)
            {
                var model = new UserAndRoleList
                {
                    user = user,
                    roller = role
                };
                return View(model);
            }
            else
            {
                return View("Error");
            }
        }


        //kullanıcı güncelleme işlemi yapan fonksiyonum
        [HttpPost]
        public async Task<IActionResult> Update(UserAndRoleList userAndRoleList)
        {
            var client = _httpClientFactory.CreateClient("apiClient");

            //gelen verilere göre dto oluşturuyorum
            var modelUser = new UserDTO()
            {
                Id = userAndRoleList.user.Id,
                Ad = userAndRoleList.user.Ad,
                Soyad = userAndRoleList.user.Soyad,
                Mail = userAndRoleList.user.Mail,
                Sifre = userAndRoleList.user.Sifre
            };
            //güncelleme işlemi için apıye istek atıyorum
            var response = await client.PutAsJsonAsync("https://localhost:7246/api/User", modelUser);

            //gelen verilere göre roledto oluşturuyorum
            var modelRole = new UserRoleDTO
            {
                RolAd = userAndRoleList.user.RolAd,
                Mail = userAndRoleList.user.Mail
            };
            //güncelleme işlemi için apıye istek atıyorum
            var responseRole = await client.PutAsJsonAsync("https://localhost:7246/api/User/UpdateUserRole", modelRole);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUserMessage"] = "User başarıyla Güncellendi!";

            }
            else
            {
                TempData["ErrorUserMessage"] = "User Güncellerken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "User");
        }


        //sile batığında gerçekleşen fonksiyonum. silme sonrası listeleme ekranına yönlendiriyoruz
        //ıd = UserId
        public async Task<IActionResult> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient("apiClient");
            //kullanıcıyı bulma amacım roolü ve user tablolarında ayrı ayrı olan iki veriyide silmek
            var responseuser = await client.GetAsync($"https://localhost:7246/api/User/{id}");
            var user = await responseuser.Content.ReadFromJsonAsync<UserDTO>();

            var responserole = await client.DeleteAsync($"https://localhost:7246/api/UserRole/{user.Mail}");

            var response = await client.DeleteAsync($"https://localhost:7246/api/User/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUserMessage"] = "User başarıyla silinmiştir!";

            }
            else
            {
                TempData["ErrorUserMessage"] = "User silinirken bir hata oluştu. Lütfen tekrar deneyin.";

            }
            return RedirectToAction("List", "User");
        }


        //kullanıcının kendi bilgilerini güncelleme kısmı
        public async Task<IActionResult> UserUpdate(string id)
        {
            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);


            //kullanıcıyı bulma
            var client = _httpClientFactory.CreateClient("apiClient");
            var response = await client.GetAsync($"https://localhost:7246/api/User/{id}");
            var userDTO = await response.Content.ReadFromJsonAsync<UserDTO>();

            KategoriAndUserDTO model = new KategoriAndUserDTO()
            {
                kategoriler = kategoriValues,
                userDTO = userDTO
            };
            return View(model);
        }


        //kullanıcının kendi bilgilerini güncelleme kısmından gelen veriler ile ekleme yapaan fonksiyonyum
        [HttpPost]
        public async Task<IActionResult> UserUpdate(UserDTO userDTO)
        {

            var client = _httpClientFactory.CreateClient("apiClient");
            var response = await client.PutAsJsonAsync("https://localhost:7246/api/User", userDTO);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUserMessage"] = "başarıyla Güncellendi!";

            }
            else
            {
                TempData["ErrorUserMessage"] = "Güncellerken bir hata oluştu. Lütfen tekrar deneyin.";

            }

            HttpContext.Session.SetString("UserAd", userDTO.Ad);
            HttpContext.Session.SetString("UserSoyad", userDTO.Soyad);
            HttpContext.Session.SetString("UserSifre", userDTO.Sifre);

            return RedirectToAction("Index", "Home");
        }


        //kullanıcı Bilgilerimden/Kurslarim butonuna bastığınnda ulaştığı ekran
        public async Task<IActionResult> Kurslarim()
        {
            var mail = HttpContext.Session.GetString("UserMail");
            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);

            //kursları bulma
            var client = _httpClientFactory.CreateClient("apiClient");
            var responseKurs = await client.GetAsync($"https://localhost:7246/api/Kurs/UserGetKurslar/{mail}");
            var kurslar = await responseKurs.Content.ReadFromJsonAsync<List<Kurs>>();

            ListKurslarAndListKategoriler model = new ListKurslarAndListKategoriler()
            {
                kategoriler = kategoriValues,
                kurslar = kurslar
            };
            return View(model);
        }


        //kullanıcı Bilgilerimden/OgrenimIceriklerim butonuna bastığınnda ulaştığı ekran
        public async Task<IActionResult> OgrenimIceriklerim()
        {
            var UserId = HttpContext.Session.GetString("UserId");
            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);

            //kursları bulma
            var client = _httpClientFactory.CreateClient("apiClient");
            var responsesatinAlinanKurs = await client.GetAsync($"https://localhost:7246/api/SatinAlinanKurs/UserId/{UserId}");
            var satinAlinanKursList = await responsesatinAlinanKurs.Content.ReadFromJsonAsync<List<SatinAlinanKurs>>();

            var kursModel = new List<Kurs>();

            foreach (var item in satinAlinanKursList)
            {
                //kursları bulma
                var responseKursList = await client.GetAsync($"https://localhost:7246/api/Kurs/SatinAlinanKurs/{item.KursId}");
                var kurslar = await responseKursList.Content.ReadFromJsonAsync<Kurs>();
                kursModel.Add(kurslar);
            }


            ListKurslarAndListKategoriler model = new ListKurslarAndListKategoriler()
            {
                kategoriler = kategoriValues,
                kurslar = kursModel
            };
            return View(model);
        }

    }
}
