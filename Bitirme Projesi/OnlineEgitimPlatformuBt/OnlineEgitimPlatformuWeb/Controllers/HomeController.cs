using Core.DTOs;
using Core.ListModels;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineEgitimPlatformuWeb.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace OnlineEgitimPlatformuWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        
        //kullanici Çıkış İşlemleri
        public async Task<IActionResult> Logout()
        {
            // Oturumu temizle
            HttpContext.Session.Clear();

            //apiye sepeti sil işlemi için istek atıyoruz
            var client = _httpClientFactory.CreateClient();
            var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
            //ana sayfaya yönlendiriyoruz
            return RedirectToAction("Index", "Home");
        }


        //ana Sayfam
        public async Task<IActionResult> Index()
        {
            // Top8 Kurslar
            var kurslarClient = _httpClientFactory.CreateClient();
            var kurslarResponse = await kurslarClient.GetAsync("https://localhost:7246/api/Kurs/Top8/");
            var kurslarJsonData = await kurslarResponse.Content.ReadAsStringAsync();
            var kurslarValues = JsonConvert.DeserializeObject<List<Kurs>>(kurslarJsonData);

            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);


            KategoriAndTopKurs ListModel = new KategoriAndTopKurs()
            {
                kurslar = kurslarValues,
                kategoriler = kategoriValues,
            };

            return View(ListModel);
        }


        //giriş veya kayit ol ekranım
        public async Task<IActionResult> LoginAndCreate()
        {
            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);

            return View(kategoriValues);
        }


        //kullanici giriş yapınca yönlendirilen fonksiyon
        public async Task<IActionResult> UserGetEmail(string mail, string sifre)
        {
            // Kullanıcıyı Bul
            var client = _httpClientFactory.CreateClient();

            var loginDto = new LoginDTO()
            {
                Mail = mail,
                Sifre = sifre
            };
            //serilazekme işlemini burda kullandım kodumun genelinde ....AsJsonAsync ile serilazsız işşlemler yaptım
            //serilaz işlemi ise kodu json datata cevirme işlemidir
            //girilen bilgileri serilaz edip jsondataya çevirip content olarak apıye gönnderecek şekilde ayarlıyoruz
            var jsondata = JsonConvert.SerializeObject(loginDto);
            var stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");

            //apiye verilen bilgileri gönderiyoruz ok dönerse bu bilgilere ait kullanıcı olduğu anlamına gelir
            var response = await client.PostAsync("https://localhost:7246/api/User/Login", stringContent);

            //ok dönmezse hata mesajı ile giriş yap kısmına yönlendir
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorGiris"] = "Girdiğiniz Bilgiler geçersiz. Lütfen tekrar deneyin.";
                return RedirectToAction("LoginAndCreate", "Home");
            }
            //gelen veriyi okuyup deserilaz ile json verisinden koda göndürüyorum
            var jsonData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<User>(jsonData);
            //kullanıcı yoksa hata mesajı ile giriş ekranına yönlendiriyorum
            if (users == null)
            {
                TempData["ErrorGiris"] = "Girdiğiniz Bilgiler geçersiz. Lütfen tekrar deneyin.";
                return RedirectToAction("LoginAndCreate", "Home");
            }

            // Rolü Bul
            var jsondataEMAİL = JsonConvert.SerializeObject(mail);

            StringContent stringContentEMAİL = new StringContent(jsondataEMAİL, Encoding.UTF8, "application/json");

            var responseEmail = await client.PostAsync($"https://localhost:7246/api/UserRole/{mail}", stringContentEMAİL);

            //rol bulunamazsa hata mesajı ile giriş ekranına tekrar yönlendiriyorum
            if (!responseEmail.IsSuccessStatusCode)
            {
                TempData["ErrorGiris"] = "Girdiğiniz Bilgiler geçersiz. Lütfen tekrar deneyin.";
                return RedirectToAction("LoginAndCreate", "Home");
            }

            //ama rolü var ise role göre sesionıma gerekli bilgileri atıyorum ve giriş ekranına yönlendiriyorum
            var jsonDataRole = await responseEmail.Content.ReadAsStringAsync();
            if (jsonDataRole == "member")
            {
                HttpContext.Session.SetString("UserId", users.Id);
                HttpContext.Session.SetString("UserAd", users.Ad);
                HttpContext.Session.SetString("UserSoyad", users.Soyad);
                HttpContext.Session.SetString("UserMail", users.Mail);
                HttpContext.Session.SetString("UserSifre", users.Sifre);
                HttpContext.Session.SetString("UserRole", jsonDataRole);
                //sepeti sil isteiği apıye atılıyor
                var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
                return RedirectToAction("Index", "Home");

            }

            if (jsonDataRole == "Admin")
            {
                HttpContext.Session.SetString("UserId", users.Id);
                HttpContext.Session.SetString("UserAd", users.Ad);
                HttpContext.Session.SetString("UserSoyad", users.Soyad);
                HttpContext.Session.SetString("UserMail", users.Mail);
                HttpContext.Session.SetString("UserSifre", users.Sifre);
                HttpContext.Session.SetString("UserRole", jsonDataRole);
                //sepeti sil isteiği apıye atılıyor
                var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
                return RedirectToAction("Index", "Home");
            }
            if (jsonDataRole == "Ogretmen")
            {
                HttpContext.Session.SetString("UserId", users.Id);
                HttpContext.Session.SetString("UserAd", users.Ad);
                HttpContext.Session.SetString("UserSoyad", users.Soyad);
                HttpContext.Session.SetString("UserMail", users.Mail);
                HttpContext.Session.SetString("UserSifre", users.Sifre);
                HttpContext.Session.SetString("UserRole", jsonDataRole);
                //sepeti sil isteiği apıye atılıyor
                var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
                return RedirectToAction("Index", "Home");
            }
            //eğer rollerde eşleşmezse tekrar hata mesaı ile girişe yönlendiriyorum
            TempData["ErrorGiris"] = "Girdiğiniz Bilgiler geçersiz. Lütfen tekrar deneyin.";
            return RedirectToAction("LoginAndCreate", "Home");
        }


        //hakimizda kısımı
        public async Task<IActionResult> Hakimizda()
        {
            // Kurslar
            var kurslarClient = _httpClientFactory.CreateClient();
            var kurslarResponse = await kurslarClient.GetAsync("https://localhost:7246/api/Kurs/");
            var kurslarJsonData = await kurslarResponse.Content.ReadAsStringAsync();
            var kurslarValues = JsonConvert.DeserializeObject<List<Kurs>>(kurslarJsonData);

            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);


            KategoriAndTopKurs ListModel = new KategoriAndTopKurs()
            {
                kurslar = kurslarValues,
                kategoriler = kategoriValues,
            };

            return View(ListModel);
        }


        //kullanici doğrulama kısmını geçtikten sonraki kısım kayıt kısmı
        public async Task<IActionResult> Create(string girilenKod)
        {
            var client = _httpClientFactory.CreateClient();

            //gecici bir süreliğine girilen bilgileri getiriyorum
            var ad = HttpContext.Session.GetString("UserAd");
            var soyad = HttpContext.Session.GetString("UserSoyad");
            var mail = HttpContext.Session.GetString("UserMail");
            
            var sifre = HttpContext.Session.GetString("UserSifre");
            var dogrulamaKodu = HttpContext.Session.GetString("DogrulamaKodu");
            //girilen kod ile gönderilen kod doğru ise kayıt işlemine devam ediyorum
            if (dogrulamaKodu == girilenKod)
            {
                UserDTO userDTO = new UserDTO()
                {
                    Ad = ad,
                    Soyad = soyad,
                    Mail = mail,
                    Sifre = sifre,
                };

                var jsondata = JsonConvert.SerializeObject(userDTO);

                StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");

                var response = await client.PostAsJsonAsync("https://localhost:7246/api/User", userDTO);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessKayit"] = "Kayıt başarıyla oluşturuldu! Giriş Yapınız.";
                    var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
                    return RedirectToAction("LoginAndCreate", "Home");
                }
                else
                {
                    TempData["ErrorKayit"] = "Kayıt oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";

                }
                return RedirectToAction("LoginAndCreate", "Home");
            }
            //değil ise hata mesajı ve kayıt kontorl ekranımayönlendiriyorum
            else
            {
                TempData["ErrorKayit"] = "Girdiğiniz kod geçersiz tekrar deneyin.";

            }
            
            return RedirectToAction("KayitKontrol", "Home");
        }
        

        //sifreyi unuttum ve kayıt olunduktan sonraki ekran 
        public async Task<IActionResult> KayitKontrol()
        {
            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);


            return View(kategoriValues);

        }
       
        
        //kullanıcı kayıt olduğunda gelen fonksiyon
        public async Task<IActionResult> KayitOncesiMailOnay(string Ad, string Soyad, string Mail, string Sifre)
        {
            var client = _httpClientFactory.CreateClient();

            //maili apiye gönderiyorum varmı yokmu kontrolünü sağlıyor
            var responseKontrol = await client.GetAsync($"https://localhost:7246/api/User/MailVarmi/{Mail}");
            var mailKontrol = await responseKontrol.Content.ReadFromJsonAsync<bool>();
            //varsa hata mesajı giriş ekranına yönlendir
            if (mailKontrol)
            {
                TempData["ErrorKayit"] = "Girilen mail mecut!!";
                return RedirectToAction("LoginAndCreate", "Home");
            }


            //şifre kontorlü üçün gerekli regexkerim ragexim burda aramaları gerçekleştirmek için kullanıyorum
            //tüm büyük harrfleri atıyorum
            var buyukHarfler = new Regex(@"[A-Z]+");
            //tüm büyük harrfleri atıyorum
            var kucukHarfler = new Regex(@"[a-z]+");
            //tüm rakamları atıyorum
            var sayilar = new Regex(@"[0-9]+");
            //sifrede bir büyük yada bir küçük yada bir rakam yok yada uzunluğu 6 dan kücük ise hata mesajı ile girişe yönlendir
            if ((!buyukHarfler.IsMatch(Sifre) || !kucukHarfler.IsMatch(Sifre) || !sayilar.IsMatch(Sifre)) || Sifre.Length < 6)
            {
                TempData["ErrorKayit"] = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermeli ve uzunluğu en az 6 karakter olmalıdır.";
                return RedirectToAction("LoginAndCreate", "Home");
            }

            
            // Rastgele bir doğrulama kodu oluşturma için atiye istekk atıyorum
            var responseSayiOlustur = await client.GetAsync("https://localhost:7246/api/User/SayiOlustur");
            var jsonDataSayiOlustur = await responseSayiOlustur.Content.ReadAsStringAsync();
            string dogrulamaKodu = JsonConvert.DeserializeObject<string>(jsonDataSayiOlustur);


            // Kullanıcıya e-posta ile doğrulama kodunu göndermek için apıye iistek
            var responseMaileKodGonder = await client.GetAsync($"https://localhost:7246/api/User/MaileKodGonder?mail={Uri.EscapeDataString(Mail)}&dogrulamaKodu={Uri.EscapeDataString(dogrulamaKodu)}");
            var jsonDataMaileKodGonder = await responseMaileKodGonder.Content.ReadAsStringAsync();
            bool isEmailSent = JsonConvert.DeserializeObject<bool>(jsonDataMaileKodGonder);

            //mail giderse true dönüyor değil ise false dönüyor 
            
            if (isEmailSent)
            {
                // E-posta başarıyla gönderildiyse, kullanıcıyı doğrulama sayfasına yönlendiriyoruz
                TempData["SuccessMessage"] = "Doğrulama kodu e-postanıza gönderildi.";

                HttpContext.Session.SetString("UserAd", Ad);
                HttpContext.Session.SetString("UserSoyad", Soyad);
                HttpContext.Session.SetString("UserMail", Mail);
                HttpContext.Session.SetString("UserSifre", Sifre);
                HttpContext.Session.SetString("DogrulamaKodu", dogrulamaKodu);

                return RedirectToAction("KayitKontrol", "Home"); // Doğrulama için bir görünüme yönlendir
            }
            else
            {
                TempData["ErrorKayit"] = "Doğrulama kodu gönderilemedi. Lütfen tekrar deneyin.";
                return RedirectToAction("LoginAndCreate", "Home"); // Kayıt formuna geri yönlendir
            }
        }


        //ilk şifreyi unuttuma basınca yönlendirildiği ekran
        public async Task<IActionResult> SifteyiUnuttum(string mail)
        {

            // Kategoriler
            var kategoriClient = _httpClientFactory.CreateClient();
            var kategoriResponse = await kategoriClient.GetAsync("https://localhost:7246/api/Kategori/");
            var kategoriJsonData = await kategoriResponse.Content.ReadAsStringAsync();
            var kategoriValues = JsonConvert.DeserializeObject<List<Kategori>>(kategoriJsonData);


            return View(kategoriValues);
        }


        //kullanıcı Sifreyi unuttuğunda gelen fonksiyon
        public async Task<IActionResult> SifteyiUnuttumMailOnay(string mail)
        {
            var client = _httpClientFactory.CreateClient();
            //mail varmı kontrolü için apıye istek
            var responseKontrol = await client.GetAsync($"https://localhost:7246/api/User/MailVarmi/{mail}");
            var mailKontrol = await responseKontrol.Content.ReadFromJsonAsync<bool>();
            if (!mailKontrol)
            {
                TempData["ErrorSifreGuncelleme"] = "Girilen mail mecut değil!!";
                return RedirectToAction("LoginAndCreate", "Home");
            }


            // Rastgele bir doğrulama kodu oluşturma 
            var responseSayiOlustur = await client.GetAsync("https://localhost:7246/api/User/SayiOlustur");
            var jsonDataSayiOlustur = await responseSayiOlustur.Content.ReadAsStringAsync();
            string dogrulamaKodu = JsonConvert.DeserializeObject<string>(jsonDataSayiOlustur);

            // Kullanıcıya e-posta ile doğrulama kodunu göndermek için apıye iistek
            var responseMaileKodGonder = await client.GetAsync($"https://localhost:7246/api/User/MaileKodGonder?mail={Uri.EscapeDataString(mail)}&dogrulamaKodu={Uri.EscapeDataString(dogrulamaKodu)}");
            var jsonDataMaileKodGonder = await responseMaileKodGonder.Content.ReadAsStringAsync();
            bool isEmailSent = JsonConvert.DeserializeObject<bool>(jsonDataMaileKodGonder);

            //mail giderse true dönüyor değil ise false dönüyor 
            if (isEmailSent)
            {
                // E-posta başarıyla gönderildiyse, kullanıcıyı doğrulama sayfasına yönlendiriyoruz
                TempData["SuccessMessage"] = "Doğrulama kodu e-postanıza gönderildi.";

                HttpContext.Session.SetString("UserMail", mail);
                HttpContext.Session.SetString("DogrulamaKodu", dogrulamaKodu);

                return RedirectToAction("KayitKontrol", "Home"); // Doğrulama için bir görünüme yönlendir
            }
            else
            {
                TempData["ErrorKayit"] = "Doğrulama kodu gönderilemedi. Lütfen tekrar deneyin.";
                return RedirectToAction("SifteyiUnuttum", "Home"); // Kayıt formuna geri yönlendir
            }

        }


        //girilen kod ve yeni şifresi sonrası yönlendirildiği fonksiyon
        public async Task<IActionResult> SifreGuncelle(string girilenKod, string yeniSifre)
        {
            var client = _httpClientFactory.CreateClient();

            //Sessionda tutulan bilgileri getiriyoruz
            var mail = HttpContext.Session.GetString("UserMail");
            var dogrulamaKodu = HttpContext.Session.GetString("DogrulamaKodu");

            //girilen kod ile verilein kod eşit ise
            if (dogrulamaKodu == girilenKod)
            {
                //kullanıcı maili ile apiye istek atıo kullanıcı bilgileini getiriyoruaz
                var responseGetUser = await client.GetAsync($"https://localhost:7246/api/User/MaileGoreUserGet/{mail}");
                var jsonDataGetUser = await responseGetUser.Content.ReadAsStringAsync();
                var GetUser = JsonConvert.DeserializeObject<User>(jsonDataGetUser);

                UserDTO userDTO = new UserDTO()
                {
                    Id = GetUser.Id,
                    Ad = GetUser.Ad,
                    Sifre = yeniSifre,
                    Soyad = GetUser.Soyad,
                    Mail = GetUser.Mail,
                };
                //kullanıcıyı güncelliyoruz yeni sifresi ile  apıye yolluyoruyz
                var responsePut = await client.PutAsJsonAsync("https://localhost:7246/api/User", userDTO);

                if (responsePut.IsSuccessStatusCode)
                {
                    TempData["SuccessSifreGuncelleme"] = "Sifre Başarıyla güncellendi. Giriş yapınız";
                    var responsesil = await client.DeleteAsync($"https://localhost:7246/api/Sepet/All");
                    return RedirectToAction("LoginAndCreate", "Home");
                }
                else
                {
                    TempData["ErrorSifreGuncelleme"] = "Sifre güncelleme başarısız. Lütfen tekrar deneyin.";

                }
                return RedirectToAction("LoginAndCreate", "Home");
            }
            else
            {
                TempData["ErrorSifreGuncelleme"] = "Girdiğiniz kod geçersiz tekrar deneyin.";

            }

            return RedirectToAction("KayitKontrol", "Home");
        }


    }

}
