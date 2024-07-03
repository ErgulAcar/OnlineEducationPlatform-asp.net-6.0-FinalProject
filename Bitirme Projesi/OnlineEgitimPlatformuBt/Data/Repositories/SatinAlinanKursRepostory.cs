using Core.IRepostories;
using Core.Models;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SatinAlinanKursRepostory : Repostory<SatinAlinanKurs>, ISatinAlinanKursRepostory
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }

        public SatinAlinanKursRepostory(AppDbContext context) : base(context)
        {
        }


        //SatinAlinanKursLar tablosunda userId ile eşleşen sonuclaru SatinAlinmaTarihi ile sırala ve dön
        public List<SatinAlinanKurs> GetUserById(string userId)
        {
            return _appDbContext.SatinAlinanKursLar.Where(k => k.UserId == userId).OrderBy(x => x.SatinAlinmaTarihi).ToList();
        }


        //SatinAlinanKursLar tablosunda kursId ile eşleşen sonuclaru SatinAlinmaTarihi ile sırala ve dön
        public List<SatinAlinanKurs> GetKursById(string kursId)
        {
            return _appDbContext.SatinAlinanKursLar.Where(k => k.KursId == kursId).OrderBy(x => x.SatinAlinmaTarihi).ToList();
        }

        //SatinAlinanKursLar tablosunda userId ve kursId ile eşleşen ilk sonucu dön
        public SatinAlinanKurs GetSatinAlinanKursRepo(string userId, string kursId)
        {
            var model = _appDbContext.SatinAlinanKursLar.FirstOrDefault(k => k.KursId == kursId && k.UserId == userId);
            return model;
        }
    }
}
