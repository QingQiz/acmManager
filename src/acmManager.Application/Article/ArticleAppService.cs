using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Article.Dto;
using acmManager.Authorization;

namespace acmManager.Article
{
    public class ArticleAppService : acmManagerAppServiceBase
    {
        private readonly ArticleManager _articleManager;

        public ArticleAppService(ArticleManager articleManager)
        {
            _articleManager = articleManager;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task CreateArticleAsync(CreateArticleInput input)
        {
            await _articleManager.Create(ObjectMapper.Map<Article>(input));
        }

        [UnitOfWork]
        public virtual async Task<GetArticleOutput> GetArticleAsync(long articleId)
        {
            var article = await _articleManager.Get(articleId);
            // TODO comment
            return ObjectMapper.Map<GetArticleOutput>(article);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task UpdateArticleAsync(UpdateArticleInput input)
        {
            var article = await _articleManager.Get(input.Id);
            if (article.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }

            ObjectMapper.Map(input, article);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task DeleteArticleAsync(long articleId)
        {
            var article = await _articleManager.Get(articleId);
            if (article.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }

            await _articleManager.Delete(articleId);
        }
    }
}