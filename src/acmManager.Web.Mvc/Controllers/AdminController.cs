using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Users;
using acmManager.Users.Dto;
using acmManager.Web.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using UserProfileViewModel = acmManager.Web.Models.Admin.UserProfileViewModel;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesUsers_Admin)]
    public class AdminController : acmManagerControllerBase
    {
        private readonly UserAppService _userAppService;

        public AdminController(UserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public virtual ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        [UnitOfWork]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_GetAll)]
        public async Task<PartialViewResult> GetAllWithFilter(GetAllUserWithFilterViewModel input)
        {
            var filter = ObjectMapper.Map<UserInfoDto>(input);
            var filterResult = await _userAppService.GetAllUserAsync(new GetAllUserInput()
            {
                Filter = filter,
                MaxResultCount = input.MaxResultCount,
                SkipCount = input.SkipCount
            });
            return PartialView("User/_UserTable", new IndexViewModel()
            {
                Users = filterResult,
                CurrentUserFilter = input
            });
        }

        [UnitOfWork]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_GetAll)]
        public async Task<ActionResult> GetUserProfile(long userId)
        {
            var userInfo = await _userAppService.GetUserAsync(userId);
            return View("User/Profile", ObjectMapper.Map<UserProfileViewModel>(userInfo));
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Update)]
        public async Task<JsonResult> UpdateUser(UserDto input)
        {
            await _userAppService.UpdateAsync(input);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Delete)]
        public async Task<JsonResult> DeleteUser(long userId)
        {
            await _userAppService.DeleteAsync(userId);
            return Json(new AjaxResponse());
        }
    }
}