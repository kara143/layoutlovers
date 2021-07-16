using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using layoutlovers.Authorization.Users;
using layoutlovers.MultiTenancy;

namespace layoutlovers.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}