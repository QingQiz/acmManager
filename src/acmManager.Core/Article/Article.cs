using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Article
{
    public class Article: FullAuditedEntity<long>
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        
        // 浏览次数
        public long ViewTimes { get; set; }
    }
}