using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Article
{
    public class Comment: FullAuditedEntity<long>
    {
        [ForeignKey("Article")]
        public long ArticleId { get; set; }
        public Article Article { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [ForeignKey("ReplyToComment")]
        public long ReplyToCommentId { get; set; }
        public Comment ReplyToComment { get; set; }
    }
}