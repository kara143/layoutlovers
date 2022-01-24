using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace layoutlovers
{
    public interface IAppManagerBase<TEntity, TPrimaryKey> : IDomainService
    {
        Task DeleteAsync(TPrimaryKey id);
        Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllListAsync();
        Task InsertRange(IEnumerable<TEntity> entities);
        Task<TEntity> InsertAndGetEntityAsync(TEntity entity);
        Task<TEntity> InsertOrUpdateAndGetEntityAsync(TEntity entity);
        Task InsertOrUpdateAsync(TEntity entity);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);
        IQueryable<TEntity> GetAll();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> InsertRangeAndGetEntitisAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllByPredicateAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);
    }
}
