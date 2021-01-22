using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Problem
{
    public class ProblemToTypeManager : PublicManagerWithoutTenant<ProblemToType, long>
    {
        public ProblemToTypeManager(IRepository<ProblemToType, long> repository) : base(repository)
        {
        }
    }
}