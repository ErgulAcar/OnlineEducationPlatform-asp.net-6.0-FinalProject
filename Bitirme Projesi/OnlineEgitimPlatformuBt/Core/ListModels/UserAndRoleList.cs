using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ListModels
{
    public class UserAndRoleList
    {
        public HomeUserAndRoleDTO?  user { get; set; }

        public List<Role>? roller{ get; set; }
    }
}
