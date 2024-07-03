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
    public class PuanlamaRepostory : Repostory<Puanlama>, IPuanlamaRepostory
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }

        public PuanlamaRepostory(AppDbContext context) : base(context)
        {


        }


        //Puanlamalar tablonunda Mail ve Mail ile eşlesen veriyi dön
        public Puanlama getMailAndKurs(string Mail, string KursId)
        {
            var model = _appDbContext.Puanlamalar.FirstOrDefault(x => x.KullaniciMail == Mail && x.KursId == KursId);
            return model;
        }


        //Puanlamalar tablonunda KursId ile eşleşen tüm verileri dön
        public List<Puanlama> KursPuaniRepo(string kursId)
        {
            var model = _appDbContext.Puanlamalar.Where(x => x.KursId == kursId).OrderBy(x => x.Puan).ToList();
            return model;
        }
    }
}
