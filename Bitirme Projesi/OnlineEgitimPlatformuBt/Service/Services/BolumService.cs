using Core.DTOs;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BolumService : Service<Bolum>, IBolumService
    {
        private readonly IService<Bolum> _service;
        private readonly IBolumRepostory _bolumRepostory;
        public BolumService(IUnitOfWork unitOfWork, IRepostory<Bolum> repostory, IService<Bolum> service, IBolumRepostory bolumRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _bolumRepostory = bolumRepostory;
        }

        public async Task<List<Bolum>> KursGetByListBolumler(string kursId)
        {
            var model = _bolumRepostory.KursGetByListBolum(kursId);
            return model;
        }

        public async Task DeleteBolumAsync(string Id)
        {
            var value = await _service.GetByIdAsync(Id);

            if (value != null)
            {
                _service.Remove(value);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task UpdateBolumAsync(BolumDTO bolumDTO)
        {
            Bolum value = await _service.GetByIdAsync(bolumDTO.Id);
            value.Baslik = bolumDTO.Baslik;
            value.Aciklama = bolumDTO.Aciklama;
            value.SıraNo = bolumDTO.SıraNo;
            _service.Update(value);
            await _unitOfWork.CommitAsync();
        }
    }
}
