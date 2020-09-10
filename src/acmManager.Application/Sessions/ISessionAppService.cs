using System.Threading.Tasks;
using Abp.Application.Services;
using acmManager.Sessions.Dto;

namespace acmManager.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
