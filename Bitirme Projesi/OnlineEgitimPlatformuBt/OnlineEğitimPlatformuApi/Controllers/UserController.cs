using Core.DTOs;
using Core.IServices;
using Core.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Net.Mail;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IUserRoleService _userRoleService;

        public UserController(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }


        //ekleme fonksiyonum
        [HttpPost]
        public async Task<IActionResult> AddUser(UserDTO userDTO)
        {
            var value = userDTO.Adapt<User>();
            value.Id = Guid.NewGuid().ToString();
            UserRole model = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                Mail = value.Mail,
                RolAd = "member"
            };
            var newUserRole = await _userRoleService.AddAsync(model);
            var newUser = await _userService.AddAsync(value);

            return Ok("Eklenmiştir.");
        }


        //Userıd ile kulanıcı getirme
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            var User = await _userService.GetByIdAsync(Id);

            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);
        }


        //rol ve user tablosunu birleştirip getirme
        [HttpGet("UserAndRole/{Id}")]
        public async Task<IActionResult> GetUserAndRoleById(string Id)
        {
            var User = await _userService.GetByIdAsync(Id);
            var role = _userRoleService.GetWithEmail(User.Mail);
            HomeUserAndRoleDTO userAndRole = new HomeUserAndRoleDTO()
            {
                Id = User.Id,
                Ad = User.Ad,
                Soyad = User.Soyad,
                Mail = User.Mail,
                Sifre = User.Sifre,
                RolAd = role
            };

            if (User == null)
            {
                return NotFound();
            }
            return Ok(userAndRole);
        }


        //tüm kullanıcıları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _userService.GetAllAsync();
            return Ok(Users);
        }


        // tüm rol ve user tablosunu birleştirip getirme
        [HttpGet("UserAndRole")]
        public async Task<IActionResult> GetAllUsersAndRole()
        {
            var values = (List<User>)await _userService.GetAllAsync();


            List<HomeUserAndRoleDTO> allUserandRole = new List<HomeUserAndRoleDTO>();

            for (int i = 0; i < values.Count(); i++)
            {
                string roles = _userRoleService.GetWithEmail(values[i].Mail);

                HomeUserAndRoleDTO SingleallUserandRole = new HomeUserAndRoleDTO()
                {
                    Id = values[i].Id,
                    Ad = values[i].Ad,
                    Soyad = values[i].Soyad,
                    Mail = values[i].Mail,
                    Sifre = values[i].Sifre,
                    RolAd = roles
                };

                allUserandRole.Add(SingleallUserandRole);
            }

            return Ok(allUserandRole);
        }


        //güncelleme
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDTO userDTO)
        {
            await _userService.UpdateUserAsync(userDTO);
            return Ok("Güncellenmiştir.");
        }
        
        
        //rool güncelleme
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UserRoleDTO userRoleDTO)
        {
            var value = _userRoleService.GetUserRoleByMail(userRoleDTO.Mail);
            value.RolAd = userRoleDTO.RolAd;
            await _userRoleService.UpdateUserRoleAsync(value);
            return Ok("Güncellenmiştir.");
        }


        //silme
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            await _userService.DeleteUserAsync(Id);
            return Ok("Silinmiştir.");
        }


        //giriş kontrol
        [HttpPost("Login")]
        public async Task<IActionResult> GetLogin(LoginDTO loginDTO)
        {
            return Ok(await _userService.GetNameWtihEmail(loginDTO));
        }


        //mail için sayı oluşturma
        [HttpGet("SayiOlustur")]
        public string sayiOluştur()
        {
            // Basitçe 6 haneli rastgele bir sayı oluşturuyoruz
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }


        //mail gönderme işlemim
        [HttpGet("MaileKodGonder")]
        public bool maileKodGonder(string mail, string dogrulamaKodu)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("onlineegitimplatformu1@gmail.com", "senyongsgchrgnvd");
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("onlineegitimplatformu1@gmail.com");
                mailMessage.To.Add(mail);
                mailMessage.Subject = "Doğrulama Kodu";
                mailMessage.Body = $"Doğrulama kodunuz: {dogrulamaKodu}";

                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        //Maile Gore User Getir
        [HttpGet("MaileGoreUserGet/{mail}")]
        public IActionResult MaileGoreUserGet(string mail)
        {
            var model = _userService.GetMailTooUser(mail);
            
            return Ok(model);
        }


        //mail varmı kontrolü
        [HttpGet("MailVarmi/{mail}")]
        public bool UserKontrol(string mail)
        {
            return _userService.userKontrol(mail);
        }
    }
}
