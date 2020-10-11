using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using acmManager.Public;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Certificate
{
    public class CertificateManager : PublicManagerWithoutTenant<Certificate, long>
    {
        public IEnumerable<Certificate> Certificates => Repository.GetAll();

        public CertificateManager(IRepository<Certificate, long> repository) : base(repository)
        {
        }

        public Task<Certificate> GetWithFile(long id)
        {
            var res = Repository.GetAll().Where(cer => cer.Id == id);
            if (!res.Any()) throw new UserFriendlyException("Certificate not exists");

            return res.Include(cer => cer.File).FirstAsync();
        }

        public Task<List<Certificate>> GetAllWithFile(Expression<Func<Certificate, bool>> lambda)
        {
            return Repository.GetAll().Where(lambda).OrderByDescending(c => c.AwardDate).Include(cer => cer.File).ToListAsync();
        }
    }
}