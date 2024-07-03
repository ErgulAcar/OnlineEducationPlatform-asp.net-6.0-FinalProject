using Core.IRepostories;
using Core.Models;
using Data.Context;
namespace Data.Repositories
{
    public class KursRepostory : Repostory<Kurs>, IKursRepostory
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }

        public KursRepostory(AppDbContext context) : base(context)
        {

        }


        //Kurslar tablosunda Puanagöre sırala ve illk 8 döndürs
        public List<Kurs> kursTop8()
        {
            return _appDbContext.Kurslar.OrderByDescending(k => k.Puan).Take(8).ToList();
        }


        //Kurslar tablosunda kursAd == Baslik || HocaAd || HocaSoyad en yakın sonuçları dön
        public List<Kurs> arama(string kursAd)
        {
            
            var kursAdlist = _appDbContext.Kurslar.Where(x => x.Baslik.Contains(kursAd) || x.HocaAd.Contains(kursAd) || x.HocaSoyad.Contains(kursAd)).ToList();

            return kursAdlist;
        }


        ////Kurslar tablosunda KategoriAd ile eşleşen tüm Kursları listele
        public List<Kurs> kategoriyeGoreKurslarGet(string kategoriAd)
        {
            var kategoriyeGoreKursList = _appDbContext.Kurslar.Where(x => x.KategoriAd == kategoriAd).ToList();

            return kategoriyeGoreKursList;
        }


        //Kurslar tablosunda userMaile göre ara bulduklarıını dön
        public List<Kurs> UserGetKurslarRepo(string userMail)
        {
            var UserGetKurslar = _appDbContext.Kurslar.Where(x => x.Mail == userMail).ToList();

            return UserGetKurslar;
        }


    }
}
