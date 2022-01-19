using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace layoutlovers
{
    public class AppManagerBase<TEntity, TPrimaryKey> : layoutloversServiceBase, IAppManagerBase<TEntity, TPrimaryKey>
        where TEntity : class,
        IEntity<TPrimaryKey>
    {
        protected readonly IRepository<TEntity, TPrimaryKey> _repository;
        public AppManagerBase(IRepository<TEntity, TPrimaryKey> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> InsertAndGetEntityAsync(TEntity entity)
        {
            var entityId = await _repository.InsertAndGetIdAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return _repository.Get(entityId);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var result = await _repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<TEntity> InsertOrUpdateAndGetEntityAsync(TEntity entity)
        {
            var entityId = await _repository.InsertOrUpdateAndGetIdAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return _repository.Get(entityId);
        }

        public async Task InsertOrUpdateAsync(TEntity entity)
        {
            await _repository.InsertOrUpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await _repository.InsertAsync(entity);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> InsertRangeAndGetEntitisAsync(IEnumerable<TEntity> entities)
        {
            var result = new List<TEntity>();
            foreach (var entity in entities)
            {
                var ent = await _repository.InsertAsync(entity);
                result.Add(ent);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await _repository.GetAllListAsync();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return _repository.GetAllIncluding(propertySelectors);
        }

        public async Task DeleteAsync(TPrimaryKey id)
        {
            await _repository.DeleteAsync(id);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await _repository.DeleteAsync(predicate);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var res = await _repository.GetAllListAsync(predicate);
            return res;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.FirstOrDefaultAsync(predicate);
        }

        public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            var entityId = await _repository.InsertAndGetIdAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return entityId;
        }
    }
}
