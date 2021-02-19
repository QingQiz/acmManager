using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.Runtime.Session;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Utils;
using acmManager.Web.Models.Article;
using acmManager.Web.Models.Shared;
using Castle.Core;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ArticleController : acmManagerControllerBase
    {
        private readonly ArticleAppService _articleAppService;
        private readonly CommentManager _commentManager;
        private readonly NotificationPublisher _notificationPublisher;
        private readonly NotificationSubscriptionManager _notificationSubscriptionManager;

        public const int PageSize = 10;
        
        public ArticleController(ArticleAppService articleAppService, CommentManager commentManager, NotificationPublisher notificationPublisher, NotificationSubscriptionManager notificationSubscriptionManager)
        {
            _articleAppService = articleAppService;
            _commentManager = commentManager;
            _notificationPublisher = notificationPublisher;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        #region remoteServiceFalse
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="replyToCommentId"></param>
        /// <returns>回复的评论的内容和更新后的id</returns>
        [UnitOfWork]
        [RemoteService(false)]
        public virtual async Task<Pair<string, long>> ReplyToComment(long replyToCommentId)
        {
            var contentInit = "Comment here...\n\n\n\n\n";
            
            if (replyToCommentId == 0) return new Pair<string, long>(contentInit, 0);
            
            var comment = await _commentManager.Get(replyToCommentId);
            var content = comment.Content.Split('\n').Select(s => "> " + s);
            contentInit = string.Join('\n', content) + "\n\n";

            if (comment.ReplyToCommentId != 0)
            {
                // union-find forest
                while (true)
                {
                    var commentFather = await _commentManager.Get(comment.ReplyToCommentId);
                    if (commentFather.ReplyToCommentId == 0)
                    {
                        replyToCommentId = comment.ReplyToCommentId;
                        break;
                    }
                    comment.ReplyToCommentId = commentFather.ReplyToCommentId;
                    comment = commentFather;
                }
            }
            return new Pair<string, long>(contentInit, replyToCommentId);
        }
        
        #endregion


        #region Pages
        
        [HttpGet, Route("/Article/Edit")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Article)]
        public IActionResult Edit()
        {
            return View(null);
        }
        
        
        [HttpGet, Route("/Article/Edit/{id}")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Article)]
        public async Task<IActionResult> Edit(long id)
        {
            var article = await _articleAppService.GetArticleAsync(id);
            
            if (article.CreatorUserId != AbpSession.GetUserId())
            {
                throw new AbpAuthorizationException("Permission Denied");
            }
            
            return View(article);
        }

        public async Task<ActionResult> Index(int page, long user, string keyword)
        {
            var filter = new GetArticleListFilter
            {
                MaxResultCount = PageSize,
                SkipCount = (page <= 1 ? 0 : page - 1) * PageSize,
                UserId = user,
                Keyword = keyword
            };
            var result = await _articleAppService.GetArticleWithFilter(filter);
            
            return View(new IndexViewModel
            {
                Articles = result,
                Filter = filter
            });
        }

        [HttpGet, Route("/Article/{articleId}")]
        public async Task<ActionResult> GetArticle(long articleId)
        {
            var article = await _articleAppService.GetArticleAsync(articleId);

            return View(article);
        }

        /// <summary>
        /// create comment page
        /// </summary>
        /// <param name="articleId"> alias of `blogId`</param>
        /// <param name="replyToCommentId"></param>
        /// <returns></returns>
        [AbpMvcAuthorize]
        [HttpGet, Route("/Article/CommentTo/{articleId}")]
        public async Task<IActionResult> CommentTo(long articleId, int replyToCommentId)
        {
            var article = await _articleAppService.GetArticleAsync(articleId);
            var reply = await ReplyToComment(replyToCommentId);

            // publish notification to all subscriber, exclude self
            await _notificationPublisher.PublishAsync(NotificationName.CommentArticle,
                new MessageNotificationData(Url.Action("GetArticle", "Article", new {ArticleId = articleId})),
                new EntityIdentifier(typeof(Blog), articleId),
                excludedUserIds: new[] {new UserIdentifier(AppConsts.DefaultTenant, AbpSession.GetUserId())});

            // subscribe comment notification
            await _notificationSubscriptionManager.SubscribeAsync(
                new UserIdentifier(AbpSession.TenantId, AbpSession.GetUserId()),
                NotificationName.CommentArticle, new EntityIdentifier(typeof(Blog), articleId));

            return View("Template/Comment/CommentTo", new CommentToViewModel
            {
                ArticleId = article.ArticleId,
                CommentTitle = article.Title,
                CommentToLink = Url.Action("GetArticle", new {ArticleId = articleId}),
                ReplyToCommentId = reply.Second,
                ReturnUrl = Url.Action("GetArticle", new {ArticleId = articleId}),
                ContentInit = reply.First
            });
        }

        #endregion
        
        #region APIs

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Article)]
        public async Task<JsonResult> Create(string title, string content)
        {
            var id = await _articleAppService.CreateArticleAsync(new CreateArticleInput
            {
                Title = title,
                Content = content
            });

            // subscribe all comment event
            await _notificationSubscriptionManager.SubscribeAsync(
                new UserIdentifier(AbpSession.TenantId, AbpSession.GetUserId()),
                NotificationName.CommentArticle, new EntityIdentifier(typeof(Blog), id));
            
            return Json(new {RedirectUrl = Url.Action("GetArticle", new {ArticleId = id})});
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Article)]
        public async Task<JsonResult> Update(long id, string title, string content)
        {
            await _articleAppService.UpdateArticleAsync(new UpdateArticleInput
            {
                Id = id,
                Title = title,
                Content = content
            });
            
            return Json(new {RedirectUrl = Url.Action("GetArticle", new {ArticleId = id})});
        }

        [HttpPost]
        public async Task<RedirectResult> CommentToArticle(long articleId, long replyToCommentId, string content, string returnUrl)
        {
            await _articleAppService.CommentToArticle(new CommentToArticleInput
            {
                ArticleId = articleId,
                ReplyToCommentId = replyToCommentId,
                Content = content
            });
            return Redirect(returnUrl);
        }
        
        public async Task<RedirectToActionResult> DeleteArticle(long id)
        {
            await _articleAppService.DeleteArticleAsync(id);

            return RedirectToAction("Index");
        }

        #endregion
    } 
}