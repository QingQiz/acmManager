using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace acmManager.Authorization
{
    public class acmManagerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
                        context.CreatePermission(PermissionNames.Pages);
            
            // register permission here, for example:
            // var userGroup = pages.CreateChildPermission(AppPermissions.Pages_UserGroup, L("PagesUserGroup"));
            // userGroup.CreateChildPermission(AppPermissions.Pages_UserGroup_AddUser, L("PagesUserGroupAddUser"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, acmManagerConsts.LocalizationSourceName);
        }
    }
}
