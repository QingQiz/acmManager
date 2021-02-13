using System.Threading.Tasks;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ArticleController : acmManagerControllerBase
    {
        private readonly ArticleAppService _articleAppService;

        public ArticleController(ArticleAppService articleAppService)
        {
            _articleAppService = articleAppService;
        }
        
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