using System.Threading.Tasks;
using Abp.Application.Services;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Users.Dto;

namespace acmManager.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<UserInfoDto> Register(RegisterInput input);
    }
}
