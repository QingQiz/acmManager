using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Session;
using acmManager.Controllers;
using acmManager.File;
using acmManager.Web.Models.Home;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : acmManagerControllerBase
    {
        private readonly FileAppService _fileAppService;

        public HomeController(FileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [AbpMvcAuthorize]
        public async Task<ActionResult> Index()
        {
            var currentUserPhoto = await _fileAppService.GetUserPhotoAsync(AbpSession.GetUserId());
            return View(new HomeViewModel()
            {
                PhotoUrl = currentUserPhoto?.FilePath
            });
        }
    }
}
