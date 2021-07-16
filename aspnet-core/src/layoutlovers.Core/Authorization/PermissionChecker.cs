using Abp.Authorization;
using layoutlovers.Authorization.Roles;
using layoutlovers.Authorization.Users;

namespace layoutlovers.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
