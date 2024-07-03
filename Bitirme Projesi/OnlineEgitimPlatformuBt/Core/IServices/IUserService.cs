using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IUserService : IService<User>
    {

        Task UpdateUserAsync(UserDTO userDTO);

        Task DeleteUserAsync(string Id);

        Task<User> GetNameWtihEmail(LoginDTO loginDTO);

        User GetMailTooUser(string Mail);

        bool userKontrol(string Mail);

    }
}
