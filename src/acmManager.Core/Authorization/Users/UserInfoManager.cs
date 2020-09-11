using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Authorization.Users
{
    public class UserInfoManager: PublicManagerWithoutTenant<UserInfo, long>
    {
        public UserInfoManager(IRepository<UserInfo, long> repository) : base(repository)
        {
        }
    }
}