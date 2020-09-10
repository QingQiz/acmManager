using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Contest
{
    public class Contest: FullAuditedEntity<long>
    {
        // 比赛名
        public string Name { get; set; }
        
        // 比赛描述
        [ForeignKey("Description")]
        public long ArticleId { get; set; }
        public Article.Article Description { get; set; }
        
        // 比赛注册开始时间
        public DateTime SignUpStartTime { get; set; }
        // 比赛注册结束时间
        public DateTime SignUpEndTime { get; set; }
        
        // 比赛结果公式
        [ForeignKey("Result")]
        public long ResultId { get; set; }
        public Article.Article Result { get; set; }
    }
}