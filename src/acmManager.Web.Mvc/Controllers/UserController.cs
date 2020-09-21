using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Authorization.Users;
using acmManager.Controllers;
using acmManager.File;
using acmManager.Users;
using acmManager.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UserController : acmManagerControllerBase
    {
        private readonly UserAppService _userAppService;
        private readonly FileAppService _fileAppService;
        private readonly UserManager _userManager;

        public const string DefaultUserPhoto = "/img/defaultUserImage.png";

        public UserController(UserAppService userAppService, FileAppService fileAppService, UserManager userManager)
        {
            _userAppService = userAppService;
            _fileAppService = fileAppService;
            _userManager = userManager;
        }

        public virtual string UserTypeToString(UserType? type)
        {
            return type switch
            {
                UserType.Administrator => "管理员",
                UserType.TempMember => "预备队员",
                UserType.RetiredMember => "退役队员",
                UserType.Member => "正式队员",
                UserType.TeamLeader => "队长",
                UserType.Teacher => "教师",
                null => "管理员",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public async Task<ActionResult> UserProfile(long userId=0)
        {
            if (userId != 0 && userId != AbpSession.GetUserId())
            {
                if (!await IsGrantedAsync(PermissionNames.PagesUsers_GetAll))
                {
                    throw new UserFriendlyException("PermissionDenied");
                }
            }
            else
            {
                userId = AbpSession.GetUserId();
            }
            var userInfo = await _userAppService.GetAsync(userId);

            var model = ObjectMapper.Map<UserProfileViewModel>(userInfo);
            model.Photo = (await _fileAppService.GetUserPhotoAsync(userInfo.UserId));
            model.CreationTime = (await _userManager.GetUserByIdAsync(userId)).CreationTime;

            return View("Profile", model);
        }
    }
}