using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Article
{
    [Table("acmMgr.Comment")]
    public class Comment: FullAuditedEntity<long>
    {
        public string Content { get; set; }
        
        // 0 to reply nothing
        public long ReplyToCommentId { get; set; }
    }
}