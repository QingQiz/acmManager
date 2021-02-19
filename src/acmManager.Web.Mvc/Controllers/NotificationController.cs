using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Controllers;
using Castle.Core;
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
            var userId = new UserIdentifier(AbpSession.TenantId, AbpSession.GetUserId());
            var notifications = (await _notificationManager.GetUserNotificationsAsync(userId))
                .GroupBy(n => n.Notification.Data["Message"])
                // reduce all same notifications
                .Select(ng =>
                {
                    var newNg = ng
                        .Where(n => n.State == UserNotificationState.Unread)
                        .OrderByDescending(n => n.Notification.CreationTime)
                        .ToList();
                    return new Pair<long, UserNotification>(newNg.Count, newNg.FirstOrDefault() ?? ng.First());
                })
                .OrderByDescending(p => p.Second.Notification.CreationTime)
                .ToList();
            
            return View(notifications);
        }

        public async Task<RedirectResult> GetNotification(Guid notificationId, string returnUrl)
        {
            var userId = new UserIdentifier(AbpSession.TenantId, AbpSession.GetUserId());
            
            var notification = await _notificationManager.GetUserNotificationAsync(AbpSession.TenantId, notificationId);

            if (AbpSession.GetUserId() != notification.UserId)
            {
                throw new UserFriendlyException("Permission Denied");
            }

            // get all same notifications
            var notifications =
                (await _notificationManager.GetUserNotificationsAsync(userId))
                .Where(n => n.State == UserNotificationState.Unread)
                .Where(n => n.Notification.Data["Message"].ToString() ==
                            notification.Notification.Data["Message"].ToString());

            foreach (var n in notifications)
            {
                await _notificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, n.Id,
                    UserNotificationState.Read);
            }
            
            return new RedirectResult(returnUrl);
        }
    }
}