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
    public class KategoriController : ControllerBase
    {
        private readonly IKategoriService _kategoriService;

        public KategoriController(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }


        //ekleme fonksiyonum
        [HttpPost]
        public async Task<IActionResult> AddKategori(KategoriDTO kategoriDTO)
        {
            var value = kategoriDTO.Adapt<Kategori>();
            value.Id = Guid.NewGuid().ToString();
            var newKategori = await _kategoriService.AddAsync(value);
            return Ok("Eklenmiştir.");
        }


        //kategoriIdye göre ders getirme fonksiyonum
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetKategoriById(string Id)
        {
            var kategori = await _kategoriService.GetByIdAsync(Id);
            if (kategori == null)
            {
                return NotFound();
            }
            return Ok(kategori);
        }


        //tüm kategorileri listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllKategoris()
        {
            var kategori = await _kategoriService.GetAllAsync();
            return Ok(kategori);
        }


        //güncelleme fonksiyonum
        [HttpPut]
        public async Task<IActionResult> UpdateKategori(Kategori kategori)
        {
            await _kategoriService.UpdateKategoriAsync(kategori);
            return Ok("Güncellenmiştir.");
        }


        //silme fonksiyonum
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteKategori(string Id)
        {
            await _kategoriService.DeleteKategoriAsync(Id);
            return Ok("Silinmiştir."); ;
        }
    }
}
