using Abp.Domain.Repositories;
using acmManager.Public;

namespace acmManager.File
{
    public class FileManager: PublicManagerWithoutTenant<File, long>
    {
        public FileManager(IRepository<File, long> repository) : base(repository)
        {
        }
    }
}