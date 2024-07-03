using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepostories
{
    public interface ISatinAlinanKursRepostory : IRepostory<SatinAlinanKurs>
    {
        List<SatinAlinanKurs> GetUserById(string userId);

        List<SatinAlinanKurs> GetKursById(string kursId);

        SatinAlinanKurs GetSatinAlinanKursRepo(string userId, string kursId);
    }
}
