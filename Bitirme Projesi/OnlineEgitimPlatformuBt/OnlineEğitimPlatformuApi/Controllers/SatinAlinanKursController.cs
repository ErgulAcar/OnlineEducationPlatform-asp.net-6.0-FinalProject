using Core.DTOs;
using Core.IServices;
using Core.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatinAlinanKursController : ControllerBase
    {
        private readonly ISatinAlinanKursService _service;

        public SatinAlinanKursController(ISatinAlinanKursService service)
        {
            _service = service;
        }


        //ekleme fonksiyonum
        [HttpPost]
        public async Task<IActionResult> AddAsync(SatinAlinanKursDTO satinAlinanKursDTO)
        {
            var value = satinAlinanKursDTO.Adapt<SatinAlinanKurs>();
            value.Id = Guid.NewGuid().ToString();
            var newModel = await _service.AddAsync(value);
            return Ok("Eklenmiştir.");
        }


        //SatinAlinanKursIdye göre SatinAlinanKurs getirme fonksiyonum
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(string Id)
        {
            var model = await _service.GetByIdAsync(Id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }


        //KursIdye göre SatinAlinanKursları getirme fonksiyonum
        [HttpGet("KursId/{Id}")]
        public IActionResult GetKursByIdAsync(string Id)
        {
            //ıd = userıd = kursId
            var model = _service.GetKursByIdAsync(Id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }


        //UserIdye göre SatinAlinanKursları getirme fonksiyonum
        [HttpGet("UserId/{Id}")]
        public IActionResult GetUserByIdAsync(string Id)
        {
            //ıd = userıd
            var model = _service.GetUserByIdAsync(Id);
            
            return Ok(model);
        }


        //userId ve kursIdye göre Kayit Sorgu işlemi yapan fonksiyonum
        [HttpGet("KayitSorgu")]
        public IActionResult KursByDetailUser(string userId, string kursId)
        {
            var model = _service.GetSatinAlinanKurs(userId, kursId);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }


        //tüm SatinAlinanKursları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var model = await _service.GetAllAsync();
            return Ok(model);
        }
    }
}
