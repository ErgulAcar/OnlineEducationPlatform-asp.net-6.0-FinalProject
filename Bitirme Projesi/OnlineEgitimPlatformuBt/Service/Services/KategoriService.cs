using Core.DTOs;
using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class KategoriService : Service<Kategori>, IKategoriService
    {
        private readonly IService<Kategori> _service;
        public KategoriService(IUnitOfWork unitOfWork, IRepostory<Kategori> repostory, IService<Kategori> service) : base(unitOfWork, repostory)
        {
            _service = service;
        }

        public async Task DeleteKategoriAsync(string Id)
        {
            var value = await _service.GetByIdAsync(Id);
            if (value != null)
            {
                Remove(value);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task UpdateKategoriAsync(Kategori kategori)
        {
            Kategori value = await _service.GetByIdAsync(kategori.Id);
            value.Ad = kategori.Ad;
            _service.Update(value);
            await _unitOfWork.CommitAsync();
        }
    }
}
