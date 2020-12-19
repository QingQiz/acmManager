using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Certificate;
using acmManager.Certificate.Dto;
using acmManager.Controllers;
using acmManager.Web.Models.Certificate;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class CertificateController : acmManagerControllerBase
    {
        private readonly CertificateAppService _certificateAppService;

        public CertificateController(CertificateAppService certificateAppService)
        {
            _certificateAppService = certificateAppService;
        }

        [AbpMvcAuthorize(PermissionNames.PagesUsers_Certificate)]
        public async Task<ActionResult> Index()
        {
            return View(new IndexViewModel
            {
                Certificates = await _certificateAppService.GetAllCertificateAsync()
            });
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Certificate_Upload)]
        public async Task<JsonResult> Upload(UploadCertificateInput input)
        {
            await _certificateAppService.UploadCertificateAsync(input);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public async Task<ActionResult> Delete(long id)
        {
            await _certificateAppService.DeleteCertificateAsync(id);
            return RedirectToAction("Index");
        }
    }
}