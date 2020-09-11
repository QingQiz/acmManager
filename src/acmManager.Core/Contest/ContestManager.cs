using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Contest
{
    public class ContestManager: PublicManagerWithoutTenant<Contest, long>
    {
        public ContestManager(IRepository<Contest, long> repository) : base(repository)
        {
        }
    }
}