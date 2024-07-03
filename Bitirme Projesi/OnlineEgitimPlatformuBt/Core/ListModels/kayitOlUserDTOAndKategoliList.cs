using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class kayitOlUserDTOAndKategoliList
    {
        public List<Kategori>? kategoriler { get; set; }

        public kayitOlUserDTO? kayitOlUserDTOs { get; set; }
    }
    
}
