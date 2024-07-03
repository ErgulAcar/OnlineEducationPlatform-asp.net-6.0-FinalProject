using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepostories
{
    public interface IPuanlamaRepostory : IRepostory<Puanlama>
    {
        Puanlama getMailAndKurs(string Mail, string KursId);


        List<Puanlama> KursPuaniRepo(string kursId);
    }
}
