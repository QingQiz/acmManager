using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace acmManager.Authorization
{
    public class acmManagerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pageUser =  context.CreatePermission(PermissionNames.PagesUsers, L("Users"));
            context.CreatePermission(PermissionNames.PagesRoles, L("Roles"));
            context.CreatePermission(PermissionNames.PagesTenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
                        context.CreatePermission(PermissionNames.Pages);
            
            // register permission here
            
            // Permissions for Pages_Users
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Admin, L("Users.Admin"));
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Create, L("Users.Create"));
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Update, L("Users.Update"));
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Delete, L("Users.Delete"));
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_GetAll, L("Users.GetAll"));
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Promote, L("Users.Promote"));
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Relegate, L("Users.Relegate"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, acmManagerConsts.LocalizationSourceName);
        }
    }
}
