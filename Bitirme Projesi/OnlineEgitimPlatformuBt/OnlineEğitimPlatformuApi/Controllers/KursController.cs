using Core.DTOs;
using Core.IServices;
using Core.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Services;
using System.Collections.Generic;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KursController : ControllerBase
    {
        private readonly IKursService _kursService;

        public KursController(IKursService kursService)
        {
            _kursService = kursService;
        }


        //ekleme fonksiyonum
        [HttpPost]
        public IActionResult AddKurs(Kurs kurs)
        {
            _kursService.CreateKursAsync(kurs);
            return Ok("Eklenmiştir.");
        }


        //KursIdye göre kurs getirme fonksiyonum
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetKursById(string Id)
        {
            var kurs = await _kursService.GetByIdAsync(Id);
            if (kurs == null)
            {
                return NotFound();
            }
            return Ok(kurs);
        }


        //tüm Kursları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllKurss()
        {
            var kurss = await _kursService.GetAllAsync();
            return Ok(kurss);
        }


        //güncelleme fonksiyonum
        [HttpPut]
        public async Task<IActionResult> UpdateKurs(KursDTO kursDTO)
        {
            await _kursService.UpdateKursAsync(kursDTO);
            return Ok("Güncellenmiştir.");
        }


        //silme fonksiyonum
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteKurs(string Id)
        {
            await _kursService.DeleteKursAsync(Id);
            return Ok("Silinmiştir."); ;
        }


        //puana göre ilk 8 getirme fonksiyornum
        [HttpGet("Top8")]
        public IActionResult Top8Kurslar()
        {
            return Ok(_kursService.Top8());
        }


        //girilenTexte yakın olan kursad/hoca ad,soyad a göre kurs getirme
        [HttpGet("Arama/{girilenText}")]
        public IActionResult Arama(string girilenText)
        {
            return Ok(_kursService.KursArama(girilenText));
        }


        //Kategori Var Mi diye kontrol sağlayan fonksiyonum
        [HttpGet("KategoriVarMi/{kategoriAd}")]
        public async Task<IActionResult> KategorideKursVarmi(string kategoriAd)
        {
            // Kategorinin altında kurs var mı diye kontrol ediyoruz

            var kursList = await _kursService.Where(x => x.KategoriAd == kategoriAd);

            bool kategoriAltindaKursVarMi = kursList.Any();

            return Ok(kategoriAltindaKursVarMi);

        }


        //Kategoriye Gore Kurs Listeleme fonksiyonum
        [HttpGet("KategoriyeGoreKursList/{kategoriAd}")]
        public IActionResult KategoriyeGoreKurslariGetir(string kategoriAd)
        {
            return Ok(_kursService.KategoriyeGoreKurslariGetir(kategoriAd));

        }


        //kullanıcıya göre Kursları getirme fonksiyonum
        [HttpGet("UserGetKurslar/{userMail}")]
        public IActionResult UserGetKurslar(string userMail)
        {

            return Ok(_kursService.UserGetKurslar(userMail));

        }


        //Satin Alinan Kursu kursId ile bulmak ve getirmek
        [HttpGet("SatinAlinanKurs/{kursId}")]
        public async Task<IActionResult> SatinAlinanKurslar(string kursId)
        {
            var model = await _kursService.GetByIdAsync(kursId);

            return Ok(model);

        }


        //Katilimci arttırma fonksiyonumn
        [HttpPut("Katilimci")]
        public async Task<IActionResult> KatilimciArttir(SatinAlinanKursDTO satinAlinanKursDTO)
        {
            await _kursService.KatilimciArtir(satinAlinanKursDTO.KursId);
            return Ok("Güncellenmiştir.");
        }


        //Puan Guncelleme fonksiyonum
        [HttpPut("PuanGuncelle")]
        public async Task<IActionResult> PuanUpdate(PuanUpdateDTO puanUpdateDTO)
        {
            await _kursService.PuanUpdate(puanUpdateDTO.puan, puanUpdateDTO.kursId);
            return Ok("Güncellenmiştir.");
        }
    }
}
