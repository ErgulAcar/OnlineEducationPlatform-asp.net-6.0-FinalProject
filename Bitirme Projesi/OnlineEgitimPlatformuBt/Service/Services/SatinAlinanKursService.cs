using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SatinAlinanKursService : Service<SatinAlinanKurs>, ISatinAlinanKursService
    {
        private readonly IService<SatinAlinanKurs> _service;

        private readonly ISatinAlinanKursRepostory _satinAlinanKursRepostory;
        public SatinAlinanKursService(IUnitOfWork unitOfWork, IRepostory<SatinAlinanKurs> repostory, IService<SatinAlinanKurs> service, ISatinAlinanKursRepostory satinAlinanKursRepostory) : base(unitOfWork, repostory)
        {
            _service = service;
            _satinAlinanKursRepostory = satinAlinanKursRepostory;
        }


        public List<SatinAlinanKurs> GetKursByIdAsync(string kursId)
        {
            var modelList = _satinAlinanKursRepostory.GetKursById(kursId);
            return modelList;
        }


        public List<SatinAlinanKurs> GetUserByIdAsync(string userId)
        {
            var modelList = _satinAlinanKursRepostory.GetUserById(userId);
            return modelList;
        }

        public SatinAlinanKurs GetSatinAlinanKurs(string userId, string kursId)
        {
            var model = _satinAlinanKursRepostory.GetSatinAlinanKursRepo(userId, kursId);
            return model;
        }


    }
}
