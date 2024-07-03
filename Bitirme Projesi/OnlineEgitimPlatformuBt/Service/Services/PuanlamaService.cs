using Core.DTOs;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using Data.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PuanlamaService : Service<Puanlama>, IPuanlamaService
    {
        private readonly IService<Puanlama> _service;

        private readonly IPuanlamaRepostory _puanlamaRepostory;

        public PuanlamaService(IUnitOfWork unitOfWork, IRepostory<Puanlama> repostory, IService<Puanlama> service, IPuanlamaRepostory puanlamaRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _puanlamaRepostory = puanlamaRepostory;
        }

        public async Task<List<Puanlama>> ReadPuanlamaaAsync()
        {
            var value = await _service.GetAllAsync();

            return (List<Puanlama>)value;
        }


        public double KursPuani(string kursId)
        {
            var value = _puanlamaRepostory.KursPuaniRepo(kursId);
            var kisiSayisi = 0;
            double toplamPuan = 0;
            if (value.Count != 0)
            {
                foreach (var item in value)
                {
                    toplamPuan = (double)(toplamPuan + item.Puan);
                    kisiSayisi++;
                }
                toplamPuan = toplamPuan / kisiSayisi;
            }
            return toplamPuan;
        }

        public async Task CreatePuanlamaAsync(PuanlamaDTO puanlamaDTO)
        {
            var model = puanlamaDTO.Adapt<Puanlama>();

            var isnull = _puanlamaRepostory.getMailAndKurs(model.KullaniciMail, model.KursId);
            if (isnull == null)
            {
                model.Id = Guid.NewGuid().ToString();

                await _service.AddAsync(model);
                await _unitOfWork.CommitAsync();

            }

        }

        public async Task DeleteAsync(string Id)
        {
            var value = await _service.GetByIdAsync(Id);
            if (value != null)
            {
                Remove(value);
                _unitOfWork.Commit();
            }
        }

        public Puanlama PuanlamısMi(string userMail, string kursId)
        {
            var model = _puanlamaRepostory.getMailAndKurs(userMail, kursId);


            return model;
        }
    }
}
