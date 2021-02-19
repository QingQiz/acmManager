using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Authorization.Accounts;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Controllers;
using acmManager.Identity;
using acmManager.MultiTenancy;
using acmManager.Utils;
using acmManager.Web.Models.Account;

namespace acmManager.Web.Controllers
{
    public class AccountController : acmManagerControllerBase
    {
        private readonly UserManager _userManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly ITenantCache _tenantCache;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly AccountAppService _accountAppService;

        public AccountController(AbpLoginResultTypeHelper abpLoginResultTypeHelper, LogInManager logInManager, SignInManager signInManager, ITenantCache tenantCache, INotificationPublisher notificationPublisher, AccountAppService accountAppService, UserManager userManager)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _tenantCache = tenantCache;
            _notificationPublisher = notificationPublisher;
            _accountAppService = accountAppService;
            _userManager = userManager;
        }

        #region Login / Logout / Register

        public ActionResult Login(string returnUrl = "")
        {
            return View(new LoginFormViewModel
            {
                ReturnUrl = NormalizeReturnUrl(returnUrl),
            });
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel)
        {
            Logger.Info($"{loginModel.Username} is logging in...");
            var loginResult = await GetLoginResultAsync(loginModel.Username, loginModel.Password, GetTenancyNameOrNull());

            await _signInManager.SignInAsync(loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse { TargetUrl = NormalizeReturnUrl(loginModel.ReturnUrl) });
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string username, string password, string tenancyName)
        {
            var u = await _userManager.GetUserByStudentNumber(username);
            var loginResult = await _logInManager.LoginAsync(u.UserName, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, username, tenancyName);
            }
        }

        [HttpPost]
        [UnitOfWork]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            var res = await _accountAppService.Register(new RegisterInput()
                {Username = model.AoxiangUsername, Password = model.AoxiangPassword});

            await _notificationPublisher.PublishAsync(NotificationName.CheckProfile, 
                new MessageNotificationData(Url.Action("UserProfile", "User")),
                userIds: new[] {new UserIdentifier(AppConsts.DefaultTenant, res.First)});
            
            return Json(new AjaxResponse(res.Second));
        }

        #endregion

        #region Helpers

        public ActionResult RedirectToAppHome()
        {
            return RedirectToAction("Index", "Home");
        }

        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

        #endregion

        #region Common

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            defaultValueBuilder ??= GetAppHomeUrl;

            if (returnUrl.IsNullOrEmpty())
            {
                return defaultValueBuilder();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return defaultValueBuilder();
        }

        #endregion
    }
}
