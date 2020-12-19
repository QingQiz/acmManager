using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using acmManager.Controllers;
using acmManager.File;
using acmManager.File.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class FileController : acmManagerControllerBase
    {
        private readonly FileAppService _fileAppService;

        public FileController(FileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [AbpMvcAuthorize]
        [IgnoreAntiforgeryToken]
        [HttpPost, Route("/Image/Upload")]
        public async Task<JsonResult> UploadImage(IFormFile input)
        {
            if (!input.ContentType.Contains("image"))
            {
                return Json(new
                {
                    success = 0,
                    message = "Wrong file type: " + input.ContentType,
                });
            }

            if (input.Length > 50 * 1024 * 1024) // 50 MB
            {
                return Json(new
                {
                    success = 0,
                    message = "image size must less than 50 MB"
                });
            }
            
            var file = ObjectMapper.Map<GetFileOutput>(await FileAppService.SaveFormFileAsync(input));

            return Json(new
            {
                success = 1,
                message = "success",
                url = file.FilePath
            });
        }

        [AbpAuthorize]
        [IgnoreAntiforgeryToken]
        [HttpPost, Route("/File/Upload")]
        public async Task<JsonResult> UploadFile(IFormFile input)
        {
            var res = ObjectMapper.Map<GetFileOutput>(await FileAppService.SaveFormFileAsync(input));
            return Json(new
            {
                filename = res.FileName,
                url = res.FilePath
            });
        }
    }
}