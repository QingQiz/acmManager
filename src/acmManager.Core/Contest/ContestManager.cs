using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Contest
{
    public class ContestManager: PublicManagerWithoutTenant<Contest, long>
    {
        public ContestManager(IRepository<Contest, long> repository) : base(repository)
        {
        }

        public new Task<Contest> Get(long id)
        {
            var res = Repository.GetAll().Where(c => c.Id == id);
            if (!res.Any()) throw new UserFriendlyException("Contest not exists");
            return res
                .Include(c => c.Description)
                .Include(c => c.Result)
                .FirstAsync();
        }
    }
}