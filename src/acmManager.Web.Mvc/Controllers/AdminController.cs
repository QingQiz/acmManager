using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using acmManager.Authorization;
using acmManager.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesUsers_Admin)]
    public class AdminController : acmManagerControllerBase
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}