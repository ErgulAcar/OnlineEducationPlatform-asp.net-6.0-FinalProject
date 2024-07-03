using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Core.IServices
{
    public interface IKursService : IService<Kurs>
    {

        Task CreateKursAsync(Kurs kurs);

        Task UpdateKursAsync(KursDTO kursDTO);

        Task DeleteKursAsync(string Id);

        

        List<Kurs> KursArama(string girilenText);

        List<Kurs> Top8();

        List<Kurs> KategoriyeGoreKurslariGetir(string kategoriAd);

        List<Kurs> UserGetKurslar(string userId);

        Task KatilimciArtir(string kursId);

        Task PuanUpdate(double puan, string kursId);

    }
}
