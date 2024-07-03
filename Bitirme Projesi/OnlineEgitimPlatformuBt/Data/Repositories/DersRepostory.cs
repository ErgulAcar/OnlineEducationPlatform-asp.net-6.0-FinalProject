using Core.IRepostories;
using Core.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DersRepostory : Repostory<Ders>, IDersRepostory
    {

        private AppDbContext _appDbContext { get => _context as AppDbContext; }

        public DersRepostory(AppDbContext context) : base(context)
        {

        }


        //Dersler tablosunda BolumId ile eşleşen tüm Dersleri listele
        public List<Ders> BolumGetByListDersler(string BolumId)
        {
            var model = _appDbContext.Dersler.Where(x => x.BolumId == BolumId).OrderBy(x => x.SıraNo).ToList();
            return model;
        }

        
    }
}
