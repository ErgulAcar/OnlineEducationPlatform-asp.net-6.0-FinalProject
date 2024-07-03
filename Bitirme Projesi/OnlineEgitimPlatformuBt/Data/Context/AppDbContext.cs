
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Userlar { get; set; }
        public DbSet<UserRole> UserRoleler { get; set; }
        public DbSet<Role> Roleler { get; set; }
        public DbSet<Kategori>  Kategoriler { get; set; }
        public DbSet<Kurs> Kurslar { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Puanlama> Puanlamalar { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<SatinAlinanKurs> SatinAlinanKursLar { get; set; }
    }
}
