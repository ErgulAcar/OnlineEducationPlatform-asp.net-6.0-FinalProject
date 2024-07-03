using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class FileKursAndKategori
    {
        public FileKursDTO? FileKursDTO { get; set; }
        public List<Kategori>? kategoriler { get; set; }
    }
}
