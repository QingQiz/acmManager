using Abp.Domain.Entities.Auditing;

namespace acmManager.Certificate
{
    public class Certificate: FullAuditedEntity<long>
    {
        // 证书名
        public string Name { get; set; }
        // 证书级别
        public string Level { get; set; }
        
        public File.File File { get; set; }
    }
}