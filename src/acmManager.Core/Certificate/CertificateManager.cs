using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.Certificate
{
    public class CertificateManager: PublicManagerWithoutTenant<Certificate, long>
    {
        public CertificateManager(IRepository<Certificate, long> repository) : base(repository)
        {
        }
    }
}