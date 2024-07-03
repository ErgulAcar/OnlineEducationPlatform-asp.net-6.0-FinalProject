using Core.DTOs;
using Core.IServices;
using Core.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YorumController : ControllerBase
    {
        private readonly IYorumService _yorumService;

        public YorumController(IYorumService yorumService)
        {
            _yorumService = yorumService;
        }


        //ekleme
        [HttpPost]
        public async Task<IActionResult> AddYorum(YorumDTO yorumDTO)
        {
            var value = yorumDTO.Adapt<Yorum>();
            value.Id = Guid.NewGuid().ToString();
            var newYorum = await _yorumService.AddAsync(value);
            return Ok("Eklenmiştir.");
        }


        //ıdye göre getirme
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetYorumById(string Id)
        {
            var yorum = await _yorumService.GetByIdAsync(Id);
            if (yorum == null)
            {
                return NotFound();
            }
            return Ok(yorum);
        }


        //tüm yorumları getyirme
        [HttpGet]
        public async Task<IActionResult> GetAllYorums()
        {
            var yorum = await _yorumService.GetAllAsync();
            return Ok(yorum);
        }


        //kursIdye göre yorumları getirme
        [HttpGet("kursId/{Id}")]
        public IActionResult GetAllYorums(string Id)
        {
            var yorum = _yorumService.KursGetYorumAllAsync(Id);
            return Ok(yorum);
        }


        //yorum silme
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteYorum(string Id)
        {
            await _yorumService.DeleteYorumAsync(Id);
            return Ok("Silinmiştir."); ;
        }


        //Kursıdye göre yorum silme
        [HttpDelete("KursaGoreDeleteYorum/{Id}")]
        public IActionResult KursaGoreDeleteYorum(string Id)
        {
            var yorum = _yorumService.KursGetYorumAllAsync(Id);
            foreach (var item in yorum)
            {
                _yorumService.DeleteYorumAsync(item.Id);
            }
            return Ok("Silinmiştir."); ;
        }
    }
}
