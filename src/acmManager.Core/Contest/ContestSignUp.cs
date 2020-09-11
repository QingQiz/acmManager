using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Contest
{
    [Table("acmMgr.ContestSignUp")]
    public class ContestSignUp: FullAuditedEntity<long>
    {
        // [ForeignKey("Contest")]
        // public long ContestId { get; set; }
        public Contest Contest { get; set; }
    }
}