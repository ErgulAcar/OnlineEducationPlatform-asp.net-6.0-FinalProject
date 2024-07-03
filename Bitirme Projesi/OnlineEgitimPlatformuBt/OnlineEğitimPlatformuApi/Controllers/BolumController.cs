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
    public class BolumController : ControllerBase
    {
        private readonly IBolumService _bolumService;

        public BolumController(IBolumService bolumService)
        {
            _bolumService = bolumService;
        }

        //ekleme fonksiyonum
        [HttpPost]
        public async Task<IActionResult> AddBolum(BolumDTO bolumDTO)
        {
            var value = bolumDTO.Adapt<Bolum>();
            value.Id = Guid.NewGuid().ToString();
            var newBolum = await _bolumService.AddAsync(value);
            return Ok("Eklenmiştir.");
        }



        //bolumIdye göre bolum getirme fonksiyonum
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBolumById(string Id)
        {
            var bolum = await _bolumService.GetByIdAsync(Id);
            if (bolum == null)
            {
                return NotFound();
            }
            return Ok(bolum);
        }


        //kursIdye göre bölümleri getirme fonksiyonum
        [HttpGet("KursGetByBolum/{Id}")]
        public async Task<IActionResult> GetAllBolums(string Id)
        {
            var bolum = await _bolumService.KursGetByListBolumler(Id);
            return Ok(bolum);
        }


        //güncelleme fonksiyonum
        [HttpPut]
        public async Task<IActionResult> UpdateBolum(BolumDTO bolumDTO)
        {
            await _bolumService.UpdateBolumAsync(bolumDTO);
            return Ok("Güncellenmiştir.");
        }


        //silme fonksiyonum
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBolum(string Id)
        {
            var value = _bolumService.GetByIdAsync(Id).Result;

            var modelList =await _bolumService.KursGetByListBolumler(value.KursId);

            // kaydırma işlemi yapıyoruz
            foreach (var bolum in modelList)
            {
                if (bolum.SıraNo > value.SıraNo)
                {
                    bolum.SıraNo--; // Sıra numarasını bir azaltıyoruz
                }
            }
            await _bolumService.DeleteBolumAsync(Id);
            return Ok("Silinmiştir."); ;
        }


        //Kurs Var Mi diye kontrol sağlayan fonksiyonum
        [HttpGet("KursVarMi/{KursId}")]
        public async Task<IActionResult> KursdaBolumVarmi(string KursId)
        {
            // Kategorinin altında kurs var mı diye kontrol ediyoruz

            var bolumList = await _bolumService.Where(x => x.KursId == KursId);

            bool value = bolumList.Any();

            return Ok(value);

        }
    }
}
