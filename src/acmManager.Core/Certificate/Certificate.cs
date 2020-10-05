using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Certificate
{
    [Table("acmMgr.Certificate")]
    public class Certificate : FullAuditedEntity<long>
    {
        // 证书名
        public string Name { get; set; }

        // 证书级别
        public CertificateLevel Level { get; set; }

        public File.File File { get; set; }
        
        // 获奖日期
        public DateTime AwardDate { get; set; }

        // 创建者即为所有者
    }
}