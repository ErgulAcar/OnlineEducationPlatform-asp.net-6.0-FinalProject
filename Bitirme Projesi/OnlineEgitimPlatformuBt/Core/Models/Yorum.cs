using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Yorum
    {
        public string Id { get; set; }
        public string? KursId { get; set; }
        public string? KullaniciId { get; set; }
        public string? Icerik { get; set; }
        public DateTime? YorumTarihi { get; set; }

    }
}
