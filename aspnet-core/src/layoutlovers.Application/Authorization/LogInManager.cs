using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using layoutlovers.Authorization.Roles;
using layoutlovers.Authorization.Users;
using layoutlovers.MultiTenancy;
using System.Threading.Tasks;
using System;
using Abp.Extensions;

namespace layoutlovers.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        //[UnitOfWork]
        public new virtual async Task<AbpLoginResult<Tenant, User>> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttemptAsync(result, result.Tenant?.Name, userNameOrEmailAddress);
            
            return result;
        }

        protected new virtual async Task<AbpLoginResult<Tenant, User>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            Tenant tenant = null;
            User user;

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                user = await UserManager.FindByNameOrEmailAsync(userNameOrEmailAddress);
            }

            if (user == null)
            {
                return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress);
            }

            tenant = await TenantRepository.FirstOrDefaultAsync(t => t.Id == user.TenantId);
            var tenantId = tenant?.Id;

            await UserManager.InitializeOptionsAsync(tenantId);
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                if (await UserManager.IsLockedOutAsync(user))
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                }

                if (!await UserManager.CheckPasswordAsync(user, plainPassword))
                {
                    if (shouldLockout)
                    {
                        if (await TryLockOutAsync(tenantId, user.Id))
                        {
                            return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                        }
                    }

                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidPassword, tenant, user);
                }

                await UserManager.ResetAccessFailedCountAsync(user);

                return await CreateLoginResultAsync(user, tenant);
            }  
        }
    }
}