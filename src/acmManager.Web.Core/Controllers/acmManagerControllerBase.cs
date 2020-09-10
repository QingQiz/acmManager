using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace acmManager.Controllers
{
    public abstract class acmManagerControllerBase: AbpController
    {
        protected acmManagerControllerBase()
        {
            LocalizationSourceName = acmManagerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
