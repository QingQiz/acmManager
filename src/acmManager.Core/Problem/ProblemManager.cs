using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Problem
{
    public class ProblemManager: PublicManagerWithoutTenant<Problem, long>
    {
        public ProblemManager(IRepository<Problem, long> repository) : base(repository)
        {
        }
    }
}