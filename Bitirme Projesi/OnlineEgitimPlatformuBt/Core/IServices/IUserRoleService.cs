using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IUserRoleService : IService<UserRole>
    {
        Task UpdateUserRoleAsync(UserRoleDTO userRoleDTO);

        Task DeleteUserRoleAsync(string mail);

        string GetWithEmail(string mail);

        UserRoleDTO GetUserRoleByMail(string mail);
    }
}
