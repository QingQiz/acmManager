using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    [Table("acmMgr.ProblemType")]
    public class ProblemType: FullAuditedEntity<long>
    {
        // 类型名称，如: DP
        [Required]
        public string Name { get; set; }
        // 类型描述，如：https://en.wikipedia.org/wiki/Dynamic_programming
        public string Description { get; set; }
        
        // problem and problem-type is many-to-many
        public ICollection<ProblemToType> Problems { get; set; }
    }
}