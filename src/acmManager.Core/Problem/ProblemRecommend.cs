using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    [Table("acmMgr.ProblemRecommend")]
    public class ProblemRecommend: FullAuditedEntity<long>
    {
        // [ForeignKey("Problem")]
        // public long ProblemId { get; set; }
        public Problem Problem { get; set; }

        public string Description { get; set; }
        
        public ICollection<RecommendVote> RecommendVotes { get; set; }
    }
}