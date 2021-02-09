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

        protected override string L(string r)
        {
            var res = base.L(r);

            if (res.Length > 2 && res[0] == '[' && res[^1] == ']')
            {
                return res[1..^1];
            }
            return res;
        }
    }
}
