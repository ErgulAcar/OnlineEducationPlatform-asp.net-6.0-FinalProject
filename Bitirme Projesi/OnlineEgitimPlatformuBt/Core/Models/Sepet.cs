using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Sepet
    {
        public string Id { get; set; }

        public string? Baslik { get; set; }

        public string? ResimUrl { get; set; }

        public int? Adet { get; set; }

        public decimal? Fiyat { get; set; }

    }
}
