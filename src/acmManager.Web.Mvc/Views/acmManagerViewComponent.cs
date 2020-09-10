using Abp.AspNetCore.Mvc.ViewComponents;

namespace acmManager.Web.Views
{
    public abstract class acmManagerViewComponent : AbpViewComponent
    {
        protected acmManagerViewComponent()
        {
            LocalizationSourceName = acmManagerConsts.LocalizationSourceName;
        }
    }
}
