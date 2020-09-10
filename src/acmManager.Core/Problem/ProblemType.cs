using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    public class ProblemType: FullAuditedEntity<long>
    {
        // 类型名称，如: DP
        [Required]
        public string Name { get; set; }
        // 类型描述，如：https://en.wikipedia.org/wiki/Dynamic_programming
        public string Description { get; set; }
    }
}