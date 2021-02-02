using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace acmManager.Problem
{
    [Table("acmMgr.ProblemToType")]
    public class ProblemToType : Entity<long>, ISoftDelete
    {
        public long ProblemId { get; set; }
        public long ProblemTypeId { get; set; }

        public bool IsDeleted { get; set; }
    }
}