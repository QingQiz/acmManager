using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Contest
{
    [Table("acmMgr.ContestSignUp")]
    public class ContestSignUp: FullAuditedEntity<long>
    {
        public Contest Contest { get; set; }
    }
}