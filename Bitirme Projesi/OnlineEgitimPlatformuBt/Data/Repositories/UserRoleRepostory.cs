using Core.DTOs;
using Core.IRepostories;
using Core.IServices;
using Core.Models;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRoleRepostory : Repostory<UserRole>, IUserRoleRepostory
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }
        public UserRoleRepostory(AppDbContext context, IRepostory<UserRole> repostory) : base(context)
        {
            
        }

        // Veritabanında email'e göre arama yap ve ilk eşleşen kaydı al ve sadece rolu dön
        public string getRoleName(string mail)
        {
            var role = _appDbContext.UserRoleler.FirstOrDefault(x => x.Mail == mail);
            return role.RolAd;
        }


        // Veritabanında email'e göre arama yap ve ilk eşleşen kaydı al ve modeli value ile dön
        public UserRoleDTO getUserRole(string mail)
        {
            
            var role = _appDbContext.UserRoleler.FirstOrDefault(x => x.Mail == mail);
            var value = new UserRoleDTO
            {
                Id = role.Id,
                Mail = role.Mail,
                RolAd = role.RolAd,
            };
            return value;
        }
    }
}
