using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Problem
{
    public class RecommendVoteManager: PublicManagerWithoutTenant<RecommendVote, long>
    {
        public RecommendVoteManager(IRepository<RecommendVote, long> repository) : base(repository)
        {
        }
    }
}