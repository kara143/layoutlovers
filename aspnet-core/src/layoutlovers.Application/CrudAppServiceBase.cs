using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace layoutlovers
{
    public class CrudAppServiceBase<TEntity
        , TEntityDto
        , TPrimaryKey
        , TGetAllInput
        , TCreateInput
        , TUpdateInput> : AsyncCrudAppService<TEntity
            , TEntityDto
            , TPrimaryKey
            , TGetAllInput
            , TCreateInput
            , TUpdateInput
            , EntityDto<TPrimaryKey>>
    where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository) { }
    }
}
