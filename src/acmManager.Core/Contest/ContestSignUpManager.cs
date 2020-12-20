using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Contest
{
    public class ContestSignUpManager: PublicManagerWithoutTenant<ContestSignUp, long>
    {
        public ContestSignUpManager(IRepository<ContestSignUp, long> repository) : base(repository)
        {
        }

        public IQueryable<ContestSignUp> MakeQuery(long userId, long contestId)
        {
            return Repository.GetAll()
                .Include(s => s.Contest)
                .Where(s => s.CreatorUserId == userId && s.Contest.Id == contestId);
            
        }

        public async Task<bool> Check(long userId, long contestId)
        {
            return await MakeQuery(userId, contestId).AnyAsync();
        }

        public new async Task DeleteAll(Expression<Func<ContestSignUp, bool>> expr)
        {
            await Repository.GetAllIncluding(s => s.Contest)
                .Where(expr)
                .ForEachAsync(s => s.IsDeleted = true);
        }

        public new async Task<List<ContestSignUp>> GetAll(Expression<Func<ContestSignUp, bool>> expr)
        {
            return await Repository
                .GetAllIncluding(s => s.Contest)
                .Where(expr)
                .ToListAsync();
        }
    }
}