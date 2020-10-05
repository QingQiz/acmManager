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

        public Task<Certificate> GetWithFile(long id)
        {
            return Repository.GetAll().Where(cer => cer.Id == id).Include(cer => cer.File).FirstAsync();
        }

        public Task<List<Certificate>> GetAllWithFile(Expression<Func<Certificate, bool>> lambda)
        {
            return Repository.GetAll().Where(lambda).Include(cer => cer.File).ToListAsync();
        }
    }
}