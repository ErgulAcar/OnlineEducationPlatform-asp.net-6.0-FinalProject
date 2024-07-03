using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class DersAndDerslerAndKategori
    {
        public DersDTO? ders { get; set; }


        public List<Kategori>? kategoriler { get; set; }


        public List<Ders>? dersler { get; set; }

        public BolumDTO? Bolum { get; set; }


    }
}
