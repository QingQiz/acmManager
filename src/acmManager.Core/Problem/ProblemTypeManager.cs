using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Problem
{
    public class ProblemTypeManager: PublicManagerWithoutTenant<ProblemType, long>
    {
        public ProblemTypeManager(IRepository<ProblemType, long> repository) : base(repository)
        {
        }
    }
}