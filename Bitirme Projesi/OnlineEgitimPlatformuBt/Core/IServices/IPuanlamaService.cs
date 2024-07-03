using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IPuanlamaService : IService<Puanlama>
    {
        Task CreatePuanlamaAsync(PuanlamaDTO puanlamaDTO);

        Task<List<Puanlama>> ReadPuanlamaaAsync();

        Task DeleteAsync(string Id);

        Puanlama PuanlamısMi(string userMail, string kursId);

        double KursPuani(string kursId);
    }
}
