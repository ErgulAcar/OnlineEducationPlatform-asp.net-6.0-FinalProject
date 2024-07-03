using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SatinAlinanKurs
    {

        //public string Id { get; set; }
        
        //public string? KursId { get; set; }

        //public string? UserId { get; set; }

        //public DateTime? SatinAlinmaTarihi { get; set; }
        public string Id { get; set; }

        public string? KursId { get; set; }

        public string? Baslik { get; set; }

        public string? ResimUrl { get; set; }

        public decimal? KursFiyat { get; set; }

        public string? UserId { get; set; }

        public string? UserAd { get; set; }

        public string? UserSoyad { get; set; }

        public string? UserMail { get; set; }

        public DateTime? SatinAlinmaTarihi { get; set; }

    }
}
