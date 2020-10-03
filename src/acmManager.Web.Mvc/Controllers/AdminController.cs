using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Authorization.Accounts.Dto;
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
            return PartialView("User/GetAllUserPartial/_UserTable", new IndexViewModel()
            {
                Users = filterResult,
                CurrentUserFilter = input
            });
        }

        [UnitOfWork]
        public async Task<ActionResult> GetUserProfile(long userId)
        {
            if (await IsGrantedAsync(PermissionNames.PagesUsers_GetOne))
            {
                var userInfo = await _userAppService.GetAsync(userId);
                return View("User/ProfilePartial/Profile", ObjectMapper.Map<UserProfileViewModel>(userInfo));
            }
            else
            {
                var userInfo = await _userAppService.GetUserInfoAsync(userId);
                var model = ObjectMapper.Map<UserProfileViewModel>(userInfo);
                model.UserId = userId;
                return View("User/ProfilePartial/Profile", model);
            }
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

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Update)]
        public async Task<JsonResult> ResetPassword(ResetPasswordViewModel input)
        {
            if (input.NewPassword != input.NewPasswordAgain)
            {
                throw new UserFriendlyException("`new password` should be same with `new password again`");
            }
            
            await _userAppService.ResetPasswordAsync(new ResetPasswordDto()
            {
                UserId = input.UserId,
                NewPassword = input.NewPassword
            });
            return Json(new AjaxResponse());
        }
    }
}