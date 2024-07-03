using Core.DTOs;
using Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IBolumService : IService<Bolum>
    {
        Task<List<Bolum>> KursGetByListBolumler(string kursId);

        Task UpdateBolumAsync(BolumDTO  bolumDTO);

        Task DeleteBolumAsync(string Id);

    }
}
