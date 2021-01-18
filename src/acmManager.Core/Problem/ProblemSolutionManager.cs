﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Problem
{
    public class ProblemSolutionManager: PublicManagerWithoutTenant<ProblemSolution, long>
    {
        public ProblemSolutionManager(IRepository<ProblemSolution, long> repository) : base(repository)
        {
        }

        public IQueryable<ProblemSolution> MakeQuery()
        {
            return Repository
                .GetAllIncluding(s => s.Problem, s => s.Solution, s => s.RecommendVotes);
        }

        public new async Task<List<ProblemSolution>> GetAll(Expression<Func<ProblemSolution, bool>> expression)
        {
                return await MakeQuery().Where(expression).ToListAsync();
        }

        public new async Task<ProblemSolution> Get(long id)
        {
            var query = MakeQuery().Where(s => s.Id == id);

            if (!await query.AnyAsync())
            {
                throw new UserFriendlyException("No such problem solution");
            }

            return await query.FirstAsync();
        }
    }
}