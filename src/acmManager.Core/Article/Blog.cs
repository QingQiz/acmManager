using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Article
{
    [Table("acmMgr.Blog")]
    public class Blog : FullAuditedEntity<long>
    {
        public Article Article { get; set; }
    }
}