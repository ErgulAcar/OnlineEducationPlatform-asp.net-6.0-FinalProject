using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PuanlamaListeDTO
    {
        public string Id { get; set; }

        public string? Baslik { get; set; }

        public string? ResimUrl { get; set; }

        public string? UserAd { get; set; }

        public string? UserSoyad { get; set; }

        public string? UserMail { get; set; }

        public int? Puan { get; set; }
    }
}
