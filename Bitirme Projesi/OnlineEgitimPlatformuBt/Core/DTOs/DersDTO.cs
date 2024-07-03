using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class DersDTO
    {
        public string? Id { get; set; }

        public string? Baslik { get; set; }

        public string? Icerik { get; set; }

        public string? VideoUrl { get; set; }

        public int? SıraNo { get; set; }

        public string? BolumId { get; set; }
    }
}
