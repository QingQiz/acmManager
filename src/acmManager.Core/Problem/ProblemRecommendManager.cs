using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Problem
{
    public class ProblemRecommendManager: PublicManagerWithoutTenant<ProblemRecommend, long>
    {
        public ProblemRecommendManager(IRepository<ProblemRecommend, long> repository) : base(repository)
        {
        }
    }
}