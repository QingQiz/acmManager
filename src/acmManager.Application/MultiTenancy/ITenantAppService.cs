using Abp.Application.Services;
using acmManager.MultiTenancy.Dto;

namespace acmManager.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

