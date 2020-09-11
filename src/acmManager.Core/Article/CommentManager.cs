using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Article
{
    public class CommentManager: PublicManagerWithoutTenant<Comment, long>
    {
        public CommentManager(IRepository<Comment, long> repository) : base(repository)
        {
        }
    }
}