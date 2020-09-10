using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace acmManager.Web.Views
{
    public abstract class acmManagerRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected acmManagerRazorPage()
        {
            LocalizationSourceName = acmManagerConsts.LocalizationSourceName;
        }
    }
}
