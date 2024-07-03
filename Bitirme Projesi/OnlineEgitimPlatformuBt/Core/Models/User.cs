using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User
    {
        public string Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        //[EmailAddress]
        public string? Mail { get; set; }
        public string? Sifre { get; set; }


    }
}
