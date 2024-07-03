using Core.DTOs;
using Core.IRepostories;
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
    public class PuanlamaController : ControllerBase
    {
        private readonly IPuanlamaService _puanlamaService;

        private readonly IPuanlamaRepostory _puanlamaRepostory;

        public PuanlamaController(IPuanlamaService puanlamaService, IPuanlamaRepostory puanlamaRepostory)
        {
            _puanlamaService = puanlamaService;
            _puanlamaRepostory = puanlamaRepostory;
        }


        //ekleme fonksiyonum
        [HttpPost]
        public async Task<IActionResult> AddPuanlama(PuanlamaDTO puanlamaDTO)
        {


            await _puanlamaService.CreatePuanlamaAsync(puanlamaDTO);
            return Ok("Eklenmiştir.");
        }


        //userMail ve kursIdye göre göre Puan Vermis Mi kontorulunu sağlayan fonksiyonum
        [HttpGet("{PuanVermisMi}")]
        public IActionResult GetPuanlamaById(string userMail, string kursId)
        {
            var puanlama = _puanlamaService.PuanlamısMi(userMail, kursId);
            if (puanlama == null)
            {
                return NotFound();
            }
            return Ok(puanlama);
        }


        //tüm Puanlamaları listeleme
        [HttpGet]
        public async Task<IActionResult> GetPuanlamaAllAsync()
        {
            var puanlama = await _puanlamaService.GetAllAsync();
            if (puanlama == null)
            {
                return NotFound();
            }
            return Ok(puanlama);
        }


        //verilen kursId ye göre Kurs Getirme fonksiyonum
        [HttpGet("KursaGoreGetir/{kursId}")]
        public async Task<IActionResult> KursaGoreGetir(string kursId)
        {
            var puanlama = _puanlamaRepostory.KursPuaniRepo(kursId);
            if (puanlama == null)
            {
                return NotFound();
            }
            return Ok(puanlama);
        }


        //kursId ye göre puan getirme
        [HttpGet("KursPuanTablo/{kursId}")]
        public double KursPuaniList(string kursId)
        {
            var value = _puanlamaService.KursPuani(kursId);
            return value;
        }


        //Kursa Gore puan silme fonksiyonum
        [HttpDelete("KursaGoreDeletePuan/{Id}")]
        public IActionResult KursaGoreDelete(string Id)
        {
            var PuanList = _puanlamaRepostory.KursPuaniRepo(Id);
            foreach (var item in PuanList)
            {
                _puanlamaService.DeleteAsync(item.Id);
            }
            return Ok("Silinmiştir."); ;
        }
    }
}
