using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Certificate;
using acmManager.Certificate.Dto;
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
        private readonly CertificateAppService _certificateAppService;

        public AdminController(UserAppService userAppService, UserTypeAppService userTypeAppService, CertificateAppService certificateAppService)
        {
            _userAppService = userAppService;
            _userTypeAppService = userTypeAppService;
            _certificateAppService = certificateAppService;
        }

        #region Get

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
            UserProfileViewModel model;
            if (await IsGrantedAsync(PermissionNames.PagesUsers_GetOne))
            {
                model = ObjectMapper.Map<UserProfileViewModel>(await _userAppService.GetAsync(userId));
            }
            else
            {
                model = ObjectMapper.Map<UserProfileViewModel>(await _userAppService.GetUserInfoAsync(userId));
                model.UserId = userId;
            }

            if (await IsGrantedAsync(PermissionNames.PagesUsers_Certificate_GetAll))
            {
                model.Certificate = await _certificateAppService.GetByUserAsync(userId);
            }

            return View("User/ProfilePartial/Profile", model);
        }


        #endregion

        #region ChangeUserTable

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

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Create)]
        public async Task<JsonResult> CreateUser(CreateUserInput inp)
        {
            await _userAppService.CreateAsync(inp);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Certificate_DeleteAll)]
        public async Task<JsonResult> DeleteCertificate(long certificateId)
        {
            await _certificateAppService.DeleteAsync(certificateId);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Certificate_GetAll)]
        public async Task<PartialViewResult> GetAllCertificateWithFilter(string name, DateTime timeStart, DateTime timeEnd, List<CertificateLevel> levels, int maxResultCount, int skipCount)
        {
            var filter = new GetAllCertificateWithFilterInput
            {
                Name = name,
                TimeStart = timeStart,
                TimeEnd = timeEnd,
                Levels = levels,
                MaxResultCount = maxResultCount,
                SkipCount = skipCount,
            };
            return PartialView("Certificate/_CertificateTable", new GetAllCertificateViewModel
            {
                Certificates = await _certificateAppService.GetWithFilter(filter),
                CurrentFilter = filter
            });
        }

        #endregion
    }
}