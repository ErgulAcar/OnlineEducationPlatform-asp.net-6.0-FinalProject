using Core.DTOs;
using Core.IImagesAndIVideosSevices;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DersService : Service<Ders>, IDersService
    {
        private readonly IService<Ders> _service;

        private readonly IDersRepostory _dersRepostory;

        public DersService(IUnitOfWork unitOfWork, IRepostory<Ders> repostory, IService<Ders> service, IDersRepostory dersRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _dersRepostory = dersRepostory;
        }

        public async Task<List<Ders>> BolumlerGetByListDersler(string bolumId)
        {
            var model = _dersRepostory.BolumGetByListDersler(bolumId);
            return model;
        }

        public async Task CreateDersAsync(Ders ders)
        {
            await _service.AddAsync(ders);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteDersAsync(string Id)
        {
            var value = await _service.GetByIdAsync(Id);
            if (value != null)
            {
                _service.Remove(value);
                await _unitOfWork.CommitAsync();
            }

        }


        public async Task UpdateDersAsync(DersDTO dersDTO)
        {
            var value = await _service.GetByIdAsync(dersDTO.Id);

            if (value != null)
            {
                value.Id = dersDTO.Id;
                value.Baslik = dersDTO.Baslik;
                value.Icerik = dersDTO.Icerik;
                value.SıraNo = dersDTO.SıraNo;
                value.BolumId = dersDTO.BolumId;
                value.VideoUrl = dersDTO.VideoUrl;

                _service.Update(value);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
