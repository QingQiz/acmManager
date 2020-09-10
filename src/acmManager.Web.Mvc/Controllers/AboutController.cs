using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using acmManager.Controllers;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : acmManagerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
