using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepostories
{
    public interface IYorumRepostory : IRepostory<Yorum>
    {

        List<Yorum> getKursYorumRepo(string kursId);
    }
}
