using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IDersService : IService<Ders>
    {
        Task CreateDersAsync(Ders ders);

        Task UpdateDersAsync(DersDTO dersDTO);

        Task DeleteDersAsync(string Id);

        Task<List<Ders>> BolumlerGetByListDersler(string bolumId);
    }
}
