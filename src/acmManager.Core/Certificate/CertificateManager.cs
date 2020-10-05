using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Certificate
{
    public class CertificateManager: PublicManagerWithoutTenant<Certificate, long>
    {
        public CertificateManager(IRepository<Certificate, long> repository) : base(repository)
        {
        }

        public Task<List<Certificate>> GetAllWithFile(Expression<Func<Certificate, bool>> lambda)
        {
            return Repository.GetAll().Include(cer => cer.File).Where(lambda).ToListAsync();
        }
    }
}