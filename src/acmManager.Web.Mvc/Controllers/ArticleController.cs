using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Web.Models.Article;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ArticleController : acmManagerControllerBase
    {
        private readonly ArticleAppService _articleAppService;

        public const int PageSize = 20;
        
        public ArticleController(ArticleAppService articleAppService)
        {
            _articleAppService = articleAppService;
        }


        #region Pages
        
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Article)]
        public IActionResult Edit()
        {
            throw new System.NotImplementedException();
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

        #endregion
        
        #region APIs
        
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

        #endregion
    } 
}