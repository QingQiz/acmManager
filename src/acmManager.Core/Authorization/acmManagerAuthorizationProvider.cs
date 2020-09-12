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
            pageUser.CreateChildPermission(PermissionNames.PagesUsers_Create, L("Users.Create"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, acmManagerConsts.LocalizationSourceName);
        }
    }
}
