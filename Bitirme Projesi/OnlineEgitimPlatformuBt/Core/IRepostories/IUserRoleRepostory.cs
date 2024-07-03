using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepostories
{
    public interface IUserRoleRepostory : IRepostory<UserRole>
    {
        string getRoleName(string mail);

        UserRoleDTO getUserRole(string mail);

        
    }

}

