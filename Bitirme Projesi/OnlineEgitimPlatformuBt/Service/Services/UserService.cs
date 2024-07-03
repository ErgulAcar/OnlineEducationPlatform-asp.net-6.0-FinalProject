using Core.DTOs;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using Data.Context;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IService<User> _service;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserRepostory _userRepostory;
        public UserService(IUnitOfWork unitOfWork, IRepostory<User> repostory, IService<User> service, IUserRoleService userRoleService, IUserRepostory userRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _userRoleService = userRoleService;
            _userRepostory = userRepostory;
        }

        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            User value = await _service.GetByIdAsync(userDTO.Id);
            value.Ad = userDTO.Ad;
            value.Soyad = userDTO.Soyad;
            if (userDTO.Mail != value.Mail)
            {
                var newUserRole = await _userRoleService.SingleOrDefault(x => x.Mail == value.Mail);
                newUserRole.Mail = userDTO.Mail;
                value.Mail = userDTO.Mail;
            }
            value.Sifre = userDTO.Sifre;
            _service.Update(value);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteUserAsync(string Id)
        {
            var User = await _service.GetByIdAsync(Id);
            if (User != null)
            {
                _service.Remove(User);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<User> GetNameWtihEmail(LoginDTO loginDTO)
        {
            var value = await _service.SingleOrDefault(x => x.Mail == loginDTO.Mail && x.Sifre == loginDTO.Sifre);

            return value;
        }

        public User GetMailTooUser(string Mail)
        {
            return _userRepostory.getMailTooUser(Mail);
        }
        public bool userKontrol(string Mail)
        {
            return _userRepostory.userKontrolRepo(Mail);
        }


    }
}
