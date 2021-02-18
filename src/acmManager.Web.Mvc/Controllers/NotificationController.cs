using System.Threading.Tasks;
using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Notifications;
using Abp.Runtime.Session;
using acmManager.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class NotificationController : acmManagerControllerBase
    {
        private readonly UserNotificationManager _notificationManager;

        public NotificationController(UserNotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public async Task<ActionResult> Index()
        {
            return View(await _notificationManager.GetUserNotificationsAsync(new UserIdentifier(AbpSession.TenantId, AbpSession.GetUserId())));

        }
    }
}