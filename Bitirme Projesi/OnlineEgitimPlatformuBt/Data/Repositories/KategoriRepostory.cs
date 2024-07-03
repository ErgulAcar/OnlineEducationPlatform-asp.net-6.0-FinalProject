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
    public class KategoriRepostory : Repostory<Kategori>, IKategoriRepostory
    {


        public KategoriRepostory(AppDbContext context) : base(context)
        {

        }

    }
}
