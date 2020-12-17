using System.Collections.Generic;
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
            CreateManyPermissions(pageUser, new []
            {
                PermissionNames.PagesUsers_Admin,
                PermissionNames.PagesUsers_Create,
                PermissionNames.PagesUsers_Update,
                PermissionNames.PagesUsers_Delete,
                PermissionNames.PagesUsers_GetAll,
                PermissionNames.PagesUsers_GetOne,
                PermissionNames.PagesUsers_Promote,
                PermissionNames.PagesUsers_Relegate
            });

            var certificate = CreatePermission(pageUser, PermissionNames.PagesUsers_Certificate);
            
            CreateManyPermissions(certificate, new []
            {
                PermissionNames.PagesUsers_Certificate_Upload,
                PermissionNames.PagesUsers_Certificate_DeleteAll,
                PermissionNames.PagesUsers_Certificate_GetAll
            });

            var article = CreatePermission(pageUser, PermissionNames.PagesUsers_Article);
            CreateManyPermissions(article, new []
            {
                PermissionNames.PagesUsers_Article_Create,
                PermissionNames.PagesUsers_Article_Delete,
                PermissionNames.PagesUsers_Article_Update
            });
            
            var contest = CreatePermission(pageUser, PermissionNames.PagesUsers_Contest);
            CreateManyPermissions(contest, new []
            {
                PermissionNames.PagesUsers_Contest_SignUp
            });
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, acmManagerConsts.LocalizationSourceName);
        }

        private static Permission CreatePermission(Permission permission, string permissionName)
        {
            return permission.CreateChildPermission(permissionName, L(permissionName));
        }

        private static void CreateManyPermissions(Permission parentPermission, IEnumerable<string> permissions)
        {
            foreach (var permission in permissions)
            {
                CreatePermission(parentPermission, permission);
            }
        }
    }
}
