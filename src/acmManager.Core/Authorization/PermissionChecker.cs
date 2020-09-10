using Abp.Authorization;
using acmManager.Authorization.Roles;
using acmManager.Authorization.Users;

namespace acmManager.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
