using Core.IRepostories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ISatinAlinanKursService : IService<SatinAlinanKurs>
    {

        List<SatinAlinanKurs> GetKursByIdAsync(string kursId);

        List<SatinAlinanKurs> GetUserByIdAsync(string userId);

        SatinAlinanKurs GetSatinAlinanKurs(string userId, string kursId);
    }
}
