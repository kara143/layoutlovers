using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;

namespace layoutlovers
{
    public interface ICrudAppServiceBase<TEntityDto
        , TPrimaryKey
        , in TGetAllInput
        , in TCreateInput
        , in TUpdateInput> : IAsyncCrudAppService<TEntityDto
            , TPrimaryKey
            , TGetAllInput
            , TCreateInput
            , TUpdateInput
            , EntityDto<TPrimaryKey>>
            , IAsyncCrudAppService<TEntityDto
            , TPrimaryKey
            , TGetAllInput
            , TCreateInput
            , TUpdateInput
            , EntityDto<TPrimaryKey>
            , EntityDto<TPrimaryKey>>
        , IApplicationService
        , ITransientDependency
         where TEntityDto : IEntityDto<TPrimaryKey>
         where TUpdateInput : IEntityDto<TPrimaryKey>
    {
    }
}
