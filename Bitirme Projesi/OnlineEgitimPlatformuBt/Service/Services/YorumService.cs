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
    public class YorumService : Service<Yorum>, IYorumService
    {
        private readonly IService<Yorum> _service;

        private readonly IYorumRepostory _yorumRepostory;
        public YorumService(IUnitOfWork unitOfWork, IRepostory<Yorum> repostory, IService<Yorum> service, IYorumRepostory yorumRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _yorumRepostory = yorumRepostory;
        }

        public async Task DeleteYorumAsync(string Id)
        {
            var value = await _service.GetByIdAsync(Id);
            if (value != null)
            {
                Remove(value);
                _unitOfWork.Commit();
            }
        }
        public List<Yorum> KursGetYorumAllAsync(string kursId)
        {
            var modelList = _yorumRepostory.getKursYorumRepo(kursId);
            return modelList;
        }
        
        public async Task UpdateYorumAsync(YorumDTO yorumDTO)
        {
            Yorum value = await _service.GetByIdAsync(yorumDTO.Id);
            value.Icerik = yorumDTO.Icerik;
            value.YorumTarihi = yorumDTO.YorumTarihi;
            _service.Update(value);
            await _unitOfWork.CommitAsync();
        }
    }
}
