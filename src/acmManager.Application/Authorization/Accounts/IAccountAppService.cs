using System.Threading.Tasks;
using Abp.Application.Services;
using acmManager.Authorization.Accounts.Dto;

namespace acmManager.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
