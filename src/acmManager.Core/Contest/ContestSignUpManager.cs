using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Contest
{
    public class ContestSignUpManager: PublicManagerWithoutTenant<ContestSignUp, long>
    {
        public ContestSignUpManager(IRepository<ContestSignUp, long> repository) : base(repository)
        {
        }
    }
}