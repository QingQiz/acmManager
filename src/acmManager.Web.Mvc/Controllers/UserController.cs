using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Authorization;
using acmManager.Authorization.Users;
using acmManager.Certificate;
using acmManager.Controllers;
using acmManager.File;
using acmManager.Problem;
using acmManager.Problem.Dto;
using acmManager.Users;
using acmManager.Users.Dto;
using acmManager.Users.Type;
using acmManager.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UserController : acmManagerControllerBase
    {
        public const string DefaultUserPhoto = "/img/defaultUserImage.png";
        private readonly FileAppService _fileAppService;
        private readonly UserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly UserTypeAppService _userTypeAppService;
        private readonly ProblemAppService _problemAppService;
        private readonly CertificateAppService _certificateAppService;
        private readonly ArticleAppService _articleAppService;

        public UserController(UserAppService userAppService, FileAppService fileAppService, UserManager userManager,
            UserTypeAppService userTypeAppService, ProblemAppService problemAppService, CertificateAppService certificateAppService, ArticleAppService articleAppService)
        {
            _userAppService = userAppService;
            _fileAppService = fileAppService;
            _userManager = userManager;
            _userTypeAppService = userTypeAppService;
            _problemAppService = problemAppService;
            _certificateAppService = certificateAppService;
            _articleAppService = articleAppService;
        }

        #region Pages

        public async Task<ActionResult> MainPage(long userId)
        {
            userId = userId == 0 ? AbpSession.GetUserId() : userId;
            
            var userInfo = await _userAppService.GetUserInfoAsync(userId);
            return View(new MainPageViewModel
            {
                UserId = userId,
                UserInfo = userInfo,
                ProblemTypes = await _problemAppService.GetAllProblemTypes(),
                ProblemSolutions = (await _problemAppService.GetAllSolutionWithFilter(new GetAllSolutionFilter
                {
                    UserId = userId,
                    MaxResultCount = int.MaxValue,
                    SkipCount = 0
                })).Solutions,
                Certificates= await _certificateAppService.GetAllCertificateSummary(userId),
                Articles = (await _articleAppService.GetArticleWithFilter(new GetArticleListFilter
                {
                    UserId = userId,
                    MaxResultCount = int.MaxValue,
                    SkipCount = 0
                })).Articles
            });
        }

        public async Task<ActionResult> UserProfile()
        {
            var userId = AbpSession.GetUserId();

            var userInfo = await _userAppService.GetAsync(userId);

            var model = ObjectMapper.Map<UserProfileViewModel>(userInfo);
            model.Photo = await _fileAppService.GetUserPhotoAsync(userInfo.UserId);
            model.CreationTime = (await _userManager.GetUserByIdAsync(userId)).CreationTime;

            return View("Profile", model);
        }

        #endregion

        #region Update User Info

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> UpdateUserProfile(UpdateUserInfoInput input)
        {
            await _userAppService.UpdateInfoAsync(input);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> EditUserPhoto(EditUserPhotoViewModel input)
        {
            await _fileAppService.UploadUserPhotoAsync(input.Photo);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> ChangeUserPassword(ChangeUserPasswordViewModel input)
        {
            if (input.NewPassword != input.NewPasswordAgain)
                throw new UserFriendlyException("`new password` should be same with `new password again`");
            await _userAppService.ChangePasswordAsync(new ChangePasswordDto
            {
                CurrentPassword = input.CurrentPassword,
                NewPassword = input.NewPassword
            });
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> UpdateFromAoxiang(UpdateFromAoxiangViewModel input)
        {
            if (input.Password != input.PasswordAgain)
                throw new UserFriendlyException("`password` should be same with `password again`");

            await _userAppService.UpdateInfoFromAoxiangAsync(new UpdateUserInfoFromAoxiangInput
            {
                Password = input.Password
            });

            return Json(new AjaxResponse());
        }

        [HttpPost]
        [UnitOfWork]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Relegate)]
        public virtual async Task<JsonResult> Relegate()
        {
            var user = await _userAppService.GetMeAsync();

            switch (user.Type)
            {
                case UserType.Member:
                    await _userTypeAppService.RetireAsync();
                    break;
                case UserType.TeamLeader:
                    await _userTypeAppService.ResignationAsync();
                    break;
                default:
                    throw new UserFriendlyException("the user does not support the operation");
            }

            return Json(new AjaxResponse());
        }

        #endregion
    }
}