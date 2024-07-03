using Core.Models;

namespace Core.IRepostories
{
    public interface IKursRepostory : IRepostory<Kurs>
    {
        List<Kurs> kursTop8();

        List<Kurs> arama(string kursAd);

        List<Kurs> kategoriyeGoreKurslarGet(string kategoriAd);

        List<Kurs> UserGetKurslarRepo(string userMail);
    }
}
