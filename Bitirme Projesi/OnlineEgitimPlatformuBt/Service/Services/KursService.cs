using Core.DTOs;
using Core.IImagesAndIVideosSevices;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;

namespace Service.Services
{
    public class KursService : Service<Kurs>, IKursService
    {
        private readonly IService<Kurs> _service;

        private readonly IImageService _imageService;

        private readonly IPuanlamaRepostory _puanlamaRepostory;

        private readonly IKursRepostory _kursRepostory;

        public KursService(IUnitOfWork unitOfWork, IRepostory<Kurs> repostory, IService<Kurs> service, IImageService imageService, IPuanlamaRepostory puanlamaRepostory, IKursRepostory kursRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _imageService = imageService;
            _puanlamaRepostory = puanlamaRepostory;
            _kursRepostory = kursRepostory;
        }


        public async Task CreateKursAsync(Kurs kurs)
        {
            await _service.AddAsync(kurs);
            _unitOfWork.Commit();
        }

        public async Task DeleteKursAsync(string Id)
        {
            var value = await _service.GetByIdAsync(Id);

            if (value != null)
            {
                _service.Remove(value);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task UpdateKursAsync(KursDTO kursDTO)
        {
            var value = await _service.GetByIdAsync(kursDTO.Id);

            if (value != null)
            {
                value.Id = kursDTO.Id;
                value.ResimUrl = kursDTO.ResimUrl;
                value.Baslik = kursDTO.Baslik;
                value.Aciklama = kursDTO.Aciklama;
                value.YayinlanmaTarihi = kursDTO.YayinlanmaTarihi;
                value.HocaAd = kursDTO.HocaAd;
                value.HocaSoyad = kursDTO.HocaSoyad;
                value.Mail = kursDTO.Mail;
                value.Fiyat = kursDTO.Fiyat;
                value.Puan = kursDTO.Puan;
                if (value.Puan == null)
                {
                    value.Puan = 0;
                }
                value.KatilimciSayisi = kursDTO.KatilimciSayisi;
                if (value.KatilimciSayisi == null)
                {
                    value.KatilimciSayisi = 0;
                }
                value.KategoriAd = kursDTO.KategoriAd;

                _service.Update(value);
                await _unitOfWork.CommitAsync();
            }
        }

        


        public List<Kurs> Top8()
        {
            var top8 = _kursRepostory.kursTop8();
            return top8;
        }

        public List<Kurs> KursArama(string girilenText)
        {
            return _kursRepostory.arama(girilenText);
        }

        public List<Kurs> KategoriyeGoreKurslariGetir(string kategoriAd)
        {
            return _kursRepostory.kategoriyeGoreKurslarGet(kategoriAd);
        }

        public List<Kurs> UserGetKurslar(string userMail)
        {
            return _kursRepostory.UserGetKurslarRepo(userMail);
        }


        public async Task KatilimciArtir(string kursId)
        {
            var value = await _service.GetByIdAsync(kursId);
            if (value != null)
            {
                value.KatilimciSayisi = value.KatilimciSayisi + 1;
                _service.Update(value);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task PuanUpdate(double puan, string kursId)
        {
            var value = await _service.GetByIdAsync(kursId);
            if (value != null)
            {
                value.Puan = puan;
                _service.Update(value);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
