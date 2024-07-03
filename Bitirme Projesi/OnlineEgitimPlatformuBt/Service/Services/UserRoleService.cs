using Core.DTOs;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserRoleService : Service<UserRole>, IUserRoleService
    {
        private readonly IService<UserRole> _service;
        private readonly IUserRoleRepostory _userRoleRepostory;
        public UserRoleService(IUnitOfWork unitOfWork, IRepostory<UserRole> repostory, IService<UserRole> service, IUserRoleRepostory userRoleRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _userRoleRepostory = userRoleRepostory;
        }

        public async Task UpdateUserRoleAsync(UserRoleDTO userRoleDTO)
        {
            var value = await _service.SingleOrDefault(x => x.Mail == userRoleDTO.Mail);
            value.RolAd = userRoleDTO.RolAd;
            _service.Update(value);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteUserRoleAsync(string mail)
        {
            var value = await _service.SingleOrDefault(x => x.Mail == mail);
            Remove(value);
            await _unitOfWork.CommitAsync();

        }

        public string GetWithEmail(string mail)
        {
            var role = _userRoleRepostory.getRoleName(mail);
            return role;
        }
        public UserRoleDTO GetUserRoleByMail(string mail)
        {
            var role = _userRoleRepostory.getUserRole(mail);
            return role;
        }
    }
}
