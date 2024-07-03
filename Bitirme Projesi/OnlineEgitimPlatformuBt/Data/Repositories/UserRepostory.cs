using Core.IRepostories;
using Core.Models;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepostory : Repostory<User>, IUserRepostory
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }
        public UserRepostory(AppDbContext context) : base(context)
        {

        }


        //Userlar tablosunda Mail ile eşleşen ilk sonucu dön
        public User getMailTooUser(string Mail)
        {
            var model = _appDbContext.Userlar.FirstOrDefault(x => x.Mail == Mail );
            return model;
        }


        //Userlar tablosunda Mail ile eşleşen birşey varmı yokmu sorgula
        public bool userKontrolRepo(string mail)
        {
            // Veritabanında email'e göre arama yap ve ilk eşleşen kaydı al
            var value = _appDbContext.UserRoleler.FirstOrDefault(x => x.Mail == mail);
            var kontrol = false;
            if (value != null)
            {
                kontrol = true;
            }
            return kontrol;
        }
    }
}
