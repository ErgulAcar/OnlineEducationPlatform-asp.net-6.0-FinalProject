using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class FileKursDTO
    {
        public string? Id { get; set; }

        public string? Baslik { get; set; }

        public string? Aciklama { get; set; }

        public IFormFile? ResimUrl { get; set; }

        public DateTime? YayinlanmaTarihi { get; set; }

        public string? HocaAd { get; set; }

        public string? HocaSoyad { get; set; }

        public string? Mail { get; set; }

        public decimal? Fiyat { get; set; }

        public double? Puan { get; set; }

        public int? KatilimciSayisi { get; set; }

        public string? KategoriAd { get; set; }
    }
}
