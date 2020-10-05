using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;

namespace acmManager.Public
{
    public class PublicManagerWithoutTenant<T1, T2> : DomainService where T1 : class, IEntity<T2>
    {
        protected readonly IRepository<T1, T2> Repository;

        public PublicManagerWithoutTenant(IRepository<T1, T2> repository)
        {
            Repository = repository;
        }

        [UnitOfWork]
        public async Task<T2> Create(T1 input)
        {
            var id = await Repository.InsertAndGetIdAsync(input);
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;
        }

        [UnitOfWork]
        public async Task Delete(T2 id)
        {
            await Repository.DeleteAsync(id);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [UnitOfWork]
        public async Task Update(T1 input)
        {
            await Repository.UpdateAsync(input);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public Task<T1> Get(T2 id)
        {
            return Repository.GetAsync(id);
        }

        public Task<List<T1>> GetAll()
        {
            return Repository.GetAllListAsync();
        }

        public Task<List<T1>> GetAll(Expression<Func<T1, bool>> lambda)
        {
            return Repository.GetAllListAsync(lambda);
        }
        
        public async Task DeleteAll(Expression<Func<T1, bool>> lambda)
        {
            await Repository.DeleteAsync(lambda);
        }
    }
}