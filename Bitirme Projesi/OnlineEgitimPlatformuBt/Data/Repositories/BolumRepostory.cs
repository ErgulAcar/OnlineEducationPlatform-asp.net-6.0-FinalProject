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
    public class BolumRepostory : Repostory<Bolum>, IBolumRepostory
    {

        private AppDbContext _appDbContext { get => _context as AppDbContext; }


        public BolumRepostory(AppDbContext context) : base(context)
        {

        }


        //bölümler tablosunda KursId ile eşleşen tüm bölümleri listele
        public List<Bolum> KursGetByListBolum(string KursId)
        {
            var model = _appDbContext.Bolumler.Where(x => x.KursId == KursId).OrderBy(x => x.SıraNo).ToList();
            return model;
        }

       

    }
}
