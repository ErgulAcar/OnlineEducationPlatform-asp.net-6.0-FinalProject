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
    public class YorumRepostory : Repostory<Yorum>, IYorumRepostory
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }
        public YorumRepostory(AppDbContext context) : base(context)
        {

        }


        //Yorumlar tablosundaki kursId ile kursId eşleşen tüm sonuçları YorumTarihine göre sırala ve dön
        public List<Yorum> getKursYorumRepo(string kursId)
        {
            var modelList = _appDbContext.Yorumlar.Where(x => x.KursId == kursId).OrderBy(x => x.YorumTarihi).ToList();
            return modelList;
        }
    }
}
