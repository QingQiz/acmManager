using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Article
{
    [Table("acmMgr.Article")]
    public class Article: FullAuditedEntity<long>
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        
        // 浏览次数
        public long ViewTimes { get; set; }
        
        public ICollection<Comment> Comments { get; set; }
    }
}