using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class YorumlarAndKursAndKategoriler
    {
        public KursDTO kurslar { get; set; }

        public List<Kategori>? kategoriler { get; set; }


        public List<YorumGorunumDTO>? yorumlar { get; set; }


    }
}
