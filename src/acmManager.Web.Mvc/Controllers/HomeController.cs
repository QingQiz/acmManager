using Microsoft.AspNetCore.Mvc;
using acmManager.Controllers;

namespace acmManager.Web.Controllers
{
    public class HomeController : acmManagerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
