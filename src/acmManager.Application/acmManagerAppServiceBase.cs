using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using acmManager.Authorization.Users;
using acmManager.MultiTenancy;

namespace acmManager
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class acmManagerAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected acmManagerAppServiceBase()
        {
            LocalizationSourceName = acmManagerConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync(bool includeRoles=false)
        {
            var currentUserId = AbpSession.GetUserId();
            if (includeRoles)
            {
                return await UserManager.GetUserByIdWithRolesAsync(currentUserId);
            }
            return await UserManager.GetUserByIdAsync(currentUserId);
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
