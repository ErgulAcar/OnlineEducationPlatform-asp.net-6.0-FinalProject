using Core.IRepostories;
using Core.IServices;
using Core.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRepostory<TEntity> _repostory;
        public Service(IUnitOfWork unitOfWork, IRepostory<TEntity> repostory)
        {
            _unitOfWork = unitOfWork;
            _repostory = repostory;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _repostory.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repostory.GetAllAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {

            var values = await _repostory.GetByIdAsync(id);
            return values;
        }

        public void Remove(TEntity entity)
        {
            _repostory.Remove(entity);
        }

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repostory.SingleOrDefault(predicate);
        }

        public void Update(TEntity entity)
        {
            _repostory.Update(entity);
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repostory.Where(predicate);
        }
    }
}
