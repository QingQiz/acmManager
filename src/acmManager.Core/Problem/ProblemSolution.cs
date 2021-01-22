using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    [Table("acmMgr.ProblemSolution")]
    public class ProblemSolution: FullAuditedEntity<long>
    {
        // [ForeignKey("Problem")]
        // public long ProblemId { get; set; }
        public Problem Problem { get; set; }

        public Article.Article Solution { get; set; }
    }
}