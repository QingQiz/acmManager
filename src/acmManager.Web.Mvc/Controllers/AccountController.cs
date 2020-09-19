using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Timing;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Authorization.Accounts;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Controllers;
using acmManager.Identity;
using acmManager.MultiTenancy;
using acmManager.Web.Models.Account;

namespace acmManager.Web.Controllers
{
    public class AccountController : acmManagerControllerBase
    {
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly ITenantCache _tenantCache;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IAccountAppService _accountAppService;

        public AccountController(AbpLoginResultTypeHelper abpLoginResultTypeHelper, LogInManager logInManager, SignInManager signInManager, ITenantCache tenantCache, INotificationPublisher notificationPublisher, IAccountAppService accountAppService)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _tenantCache = tenantCache;
            _notificationPublisher = notificationPublisher;
            _accountAppService = accountAppService;
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
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "")
        {
            var loginResult = await GetLoginResultAsync(loginModel.Username, loginModel.Password, GetTenancyNameOrNull());

            await _signInManager.SignInAsync(loginResult.Identity, loginModel.RememberMe);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return Json(new AjaxResponse { TargetUrl = NormalizeReturnUrl(returnUrl) });
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string username, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(username, password, tenancyName);

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
            return Json(new AjaxResponse(res));
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

        #region Etc

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [AbpMvcAuthorize]
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                    "App.SimpleMessage",
                    new MessageNotificationData(message),
                    severity: NotificationSeverity.Info,
                    userIds: new[] { defaultTenantAdmin, hostAdmin }
                 );

            return Content("Sent notification: " + message);
        }

        #endregion
    }
}
