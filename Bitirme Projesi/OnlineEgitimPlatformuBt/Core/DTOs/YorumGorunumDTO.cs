using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class YorumGorunumDTO
    {
        public string? Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Mail { get; set; }

        public string? Icerik { get; set; }
        public DateTime? YorumTarihi { get; set; }
        public bool ErisimTipi { get; set; }
    }
}
