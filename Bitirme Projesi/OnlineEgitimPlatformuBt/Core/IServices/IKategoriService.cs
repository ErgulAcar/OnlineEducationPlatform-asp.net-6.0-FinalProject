using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IKategoriService : IService<Kategori>
    {
        Task UpdateKategoriAsync(Kategori kategori);

        Task DeleteKategoriAsync(string Id);
    }
}
