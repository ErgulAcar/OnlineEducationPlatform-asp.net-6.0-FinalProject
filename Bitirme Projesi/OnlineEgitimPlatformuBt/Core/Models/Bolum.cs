using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Bolum
    {
        public string Id { get; set; }

        public string? Baslik { get; set; }

        public string? Aciklama { get; set; }

        public int? SıraNo { get; set; }

        public string? KursId { get; set; }
    }
}
