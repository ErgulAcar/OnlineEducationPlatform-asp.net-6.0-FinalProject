using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class KategoriAndUserDTO
    {
        public UserDTO? userDTO { get; set; }

        public List<Kategori>? kategoriler { get; set; }
    }
}
