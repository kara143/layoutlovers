using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace layoutlovers.Authorization.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Admin,
                    MultiTenancySides.Host,
                    grantAllPermissionsByDefault: true)
                );

            //Static tenant roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant,
                    grantAllPermissionsByDefault: true)
                );

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.User,
                    MultiTenancySides.Tenant)
                );

            var subscriptionManager = new StaticRoleDefinition(
                    StaticRoleNames.Tenants.SubscriptionManager,
                    MultiTenancySides.Tenant)
            {
                GrantedPermissions = { AppPermissions.Pages_Administration_Tenant_SubscriptionManagement }
            };

            roleManagementConfig.StaticRoles.Add(subscriptionManager);
        }
    }
}
