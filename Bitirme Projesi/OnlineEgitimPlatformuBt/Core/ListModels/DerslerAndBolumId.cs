using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class DerslerAndBolumId
    {
        public List<Ders>? dersler { get; set; }
        public string? bolumId { get; set; }
        public string? kursId { get; set; }
    }
}
