using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class SepetListAndKategoriList
    {
        public List<Sepet>? sepetler { get; set; }
        public List<Kategori>? kategoriler { get; set; }

        public decimal? ToplamFiyat { get; set; }
    }
}
