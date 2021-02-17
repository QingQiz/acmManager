using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Article
{
    public class BlogManager : PublicManagerWithoutTenant<Blog, long>
    {
        public BlogManager(IRepository<Blog, long> repository) : base(repository)
        {
        }
        
        public new IQueryable<Blog> GetAll()
        {
            return Repository
                .GetAll()
                .Include(b => b.Article)
                .ThenInclude(a => a.Comments);
        }

        public new async Task<Blog> Get(long id)
        {
            return await GetAll().FirstAsync(b => b.Id == id);
        }
    }
}