using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController( IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }


        //ıd göre getirme
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserRoleById(string Id)
        {
            var User = await _userRoleService.GetByIdAsync(Id);

            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);
        }


        //maile göre rol getirme
        [HttpPost("{mail}")]
        public IActionResult GetUserRoleByMail(string mail)
        {
            var value = _userRoleService.GetWithEmail(mail);
            return Ok(value);
        }


        //tüm kulllanıcı rollerini getirme
        [HttpGet]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var Users = await _userRoleService.GetAllAsync();
            return Ok(Users);
        }


        //maile göre kullanıcı rolü silme
        [HttpDelete("{mail}")]
        public async Task<IActionResult> DelateAsync(string mail)
        {
            var value = _userRoleService.DeleteUserRoleAsync(mail);
            return Ok("Silinmiştir.");
        }

    }
}
