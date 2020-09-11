using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Article
{
    public class ArticleManager: PublicManagerWithoutTenant<Article, long>
    {
        public ArticleManager(IRepository<Article, long> repository) : base(repository)
        {
        }
    }
}