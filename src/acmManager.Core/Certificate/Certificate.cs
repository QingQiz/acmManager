using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Certificate
{
    [Table("acmMgr.Certificate")]
    public class Certificate: FullAuditedEntity<long>
    {
        // 证书名
        public string Name { get; set; }
        // 证书级别
        public string Level { get; set; }
        
        public File.File File { get; set; }
        
        // 创建者即为所有者
    }
}