using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    public class RecommendVote: FullAuditedEntity<long>
    {
        [ForeignKey("Recommend")]
        public long RecommendId { get; set; }
        public ProblemRecommend Recommend { get; set; }
        
        public RecommendVote Type { get; set; }
        
    }
}