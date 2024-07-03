using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Puanlama
    {
        public string Id { get; set; }

        public string? KursId { get; set; } 

        public string? KullaniciMail { get; set; } 

        public int? Puan { get; set; }



    }
}
