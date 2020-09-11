using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    [Table("acmMgr.Problem")]
    public class Problem: FullAuditedEntity<long>
    {
        // 题目名
        [Required]
        public string Name { get; set; }
        // 题目链接
        [Required]
        public string Url { get; set; }

        public ICollection<ProblemType> Types { get; set; }
    }
}