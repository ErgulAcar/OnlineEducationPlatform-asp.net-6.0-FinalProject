using Core.IServices;
using Core.Models;
using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineEğitimPlatformuApi.Seeds
{
    public class PermissionSeed
    {

        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();


            if (!dbContext.Roleler.Any())
            {
                Role member = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ad = "member"
                };
                await dbContext.Roleler.AddAsync(member);
                await dbContext.SaveChangesAsync();

                Role admin = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ad = "Admin"
                };
                await dbContext.Roleler.AddAsync(admin);
                await dbContext.SaveChangesAsync();

                Role ogretmen = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Ad = "Ogretmen"
                };
                await dbContext.Roleler.AddAsync(ogretmen);
                await dbContext.SaveChangesAsync();
            }


            if (!dbContext.Userlar.Any(x => x.Mail == "Admin"))
            {
                User user = new User()
                {
                    Ad = "Admin",
                    Soyad = "Admin",
                    Mail = "Admin",
                    Sifre = "Admin",
                    Id = Guid.NewGuid().ToString()
                };

                await dbContext.Userlar.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.UserRoleler.Any(x => x.Mail == "Admin"))
            {
                UserRole userRole = new UserRole()
                {
                    Mail = "Admin",
                    RolAd = "Admin",
                    Id = Guid.NewGuid().ToString()
                };
                await dbContext.UserRoleler.AddAsync(userRole);
                await dbContext.SaveChangesAsync();
            }




        }
    }
}
