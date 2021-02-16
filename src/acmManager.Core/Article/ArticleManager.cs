using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Article
{
    public class ArticleManager: PublicManagerWithoutTenant<Article, long>
    {
        public ArticleManager(IRepository<Article, long> repository) : base(repository)
        {
        }

        /// <summary>
        /// Get Article including its comments
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public new async Task<Article> Get(long articleId)
        {
            var query = Repository.GetAllIncluding(a => a.Comments);
            if (!await query.AnyAsync())
            {
                throw new UserFriendlyException("No such article");
            }

            return await query.Where(a => a.Id == articleId).FirstAsync();
        }

        public new IEnumerable<Article> GetAll()
        {
            return Repository.GetAll();
        }
    }
}