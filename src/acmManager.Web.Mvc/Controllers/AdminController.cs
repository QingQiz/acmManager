using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Controllers;
using acmManager.Users;
using acmManager.Users.Dto;
using acmManager.Users.Type;
using acmManager.Web.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using UserProfileViewModel = acmManager.Web.Models.Admin.UserProfileViewModel;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.PagesUsers_Admin)]
    public class AdminController : acmManagerControllerBase
    {
        private readonly UserAppService _userAppService;
        private readonly UserTypeAppService _userTypeAppService;

        public AdminController(UserAppService userAppService, UserTypeAppService userTypeAppService)
        {
            _userAppService = userAppService;
            _userTypeAppService = userTypeAppService;
        }

        public virtual ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        #region GetAllUser

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
        

        #endregion

        #region UserProfile

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

        #endregion

        #region UserPromote

        [AbpMvcAuthorize(PermissionNames.PagesUsers_GetAll)]
        public async Task<PartialViewResult> UserPromoteFilter(GetAllUserWithFilterViewModel input)
        {
            var filter = ObjectMapper.Map<UserInfoDto>(input);
            // only find TempMember
            filter.Type = UserType.TempMember;
            var res = await _userAppService.GetAllUserAsync(new GetAllUserInput()
            {
                Filter = filter,
                MaxResultCount = input.MaxResultCount,
                SkipCount = input.SkipCount
            });
            return PartialView("User/UserPromotePartial/_UserPromote", new IndexViewModel()
            {
                CurrentUserFilter = input,
                Users = res
            });
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Promote)]
        public async Task<JsonResult> UserPromote(List<long> ids)
        {
            foreach (var userId in ids)
            {
                await _userTypeAppService.ToMemberAsync(userId);
            }
            return Json(new AjaxResponse());
        }

        #endregion

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Create)]
        public async Task<JsonResult> CreateUser(CreateUserInput inp)
        {
            await _userAppService.CreateAsync(inp);
            return Json(new AjaxResponse());
        }
    }
}