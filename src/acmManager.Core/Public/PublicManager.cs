using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace acmManager.Public
{
    public class PublicManager<T1, T2>: PublicManagerWithoutTenant<T1, T2> where T1 : class, IEntity<T2>, IMustHaveTenant
    {
        public PublicManager(IRepository<T1, T2> repository) : base(repository)
        {
        }

        [UnitOfWork]
        public new async Task<T2> Create(T1 input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var id = await Repository.InsertAndGetIdAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
                return id;
            }
        }

        [UnitOfWork]
        public new async Task Update(T1 input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                await Repository.UpdateAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
    }
}