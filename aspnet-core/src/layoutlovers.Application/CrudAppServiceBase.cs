using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using layoutlovers.Authorization.Users;
using layoutlovers.MultiTenancy;
using System;
using System.Threading.Tasks;

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
        public UserManager UserManager { get; set; }
        public TenantManager TenantManager { get; set; }

        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new UserFriendlyException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }

        protected virtual Tenant GetCurrentTenantWithEdition()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdWithEdition(AbpSession.GetTenantId());
            }
        }

        protected virtual async Task<int> GetEditionId()
        {
            var tenant = await GetCurrentTenantAsync();
            if (!tenant.EditionId.HasValue)
            {
                throw new UserFriendlyException($"The tenant with Id {tenant.Id} dont have edition.");
            }

            var editionId = (int)tenant.EditionId;

            return editionId;
        }
    }
}
