using Core.DTOs;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DersController : ControllerBase
    {
        private readonly IDersService _dersService;

        public DersController(IDersService dersService)
        {
            _dersService = dersService;
        }


        //ekleme fonksiyonum
        [HttpPost]
        public async Task<IActionResult> AddDers(Ders ders)
        {
            var newDers = _dersService.CreateDersAsync(ders);
            return Ok("Eklenmiştir.");
        }


        //DersIdye göre ders getirme fonksiyonum
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDersById(string Id)
        {
            var ders = await _dersService.GetByIdAsync(Id);
            if (ders == null)
            {
                return NotFound();
            }
            return Ok(ders);
        }


        //bölümIdye göre dersleri getirme fonksiyonum
        [HttpGet("BolumGetByDersler/{Id}")]
        public async Task<IActionResult> GetAllDerss(string Id)
        {
            var dersler = await _dersService.BolumlerGetByListDersler(Id);
            return Ok(dersler);
        }


        //güncelleme fonksiyonum
        [HttpPut]
        public async Task<IActionResult> UpdateDers(DersDTO dersDTO)
        {
            await _dersService.UpdateDersAsync(dersDTO);
            return Ok("Güncellenmiştir.");
        }


        //silme fonksiyonum
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteDers(string Id)
        {
            var value = await _dersService.GetByIdAsync(Id);

            var modelList = await _dersService.BolumlerGetByListDersler(value.BolumId);

            foreach (var item in modelList)
            {
                if (item.SıraNo > value.SıraNo)
                {
                    item.SıraNo--;
                }
            }


            await _dersService.DeleteDersAsync(Id);
            return Ok("Silinmiştir."); ;
        }


        //Bolum Var Mi diye kontrol sağlayan fonksiyonum
        [HttpGet("BolumVarMi/{BolumId}")]
        public async Task<IActionResult> BolumdeDersVarmi(string BolumId)
        {
            // Kategorinin altında kurs var mı diye kontrol ediyoruz

            var dersList = await _dersService.Where(x => x.BolumId == BolumId);

            bool value = dersList.Any();

            return Ok(value);

        }
    }
}
