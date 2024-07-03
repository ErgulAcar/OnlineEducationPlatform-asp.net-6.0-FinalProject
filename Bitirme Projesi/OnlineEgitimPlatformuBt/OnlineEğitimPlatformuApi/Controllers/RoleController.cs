using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IService<Role> _service;

        public RoleController(IService<Role> service)
        {
            _service = service;
        }


        //tüm rolleri listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _service.GetAllAsync();
            return Ok(roles);
        }
    }
}
