using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IYorumService : IService<Yorum>
    {
        Task UpdateYorumAsync(YorumDTO yorumDTO);


        List<Yorum> KursGetYorumAllAsync(string kursId);


        Task DeleteYorumAsync(string Id);
    }
}
