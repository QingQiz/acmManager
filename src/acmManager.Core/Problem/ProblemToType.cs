using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace acmManager.Problem
{
    [Table("acmMgr.ProblemToType")]
    public class ProblemToType : Entity<long>
    {
        public long ProblemId { get; set; }
        public long ProblemTypeId { get; set; }
    }
}