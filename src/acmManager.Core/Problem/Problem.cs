using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Problem
{
    public class Problem: FullAuditedEntity<long>
    {
        // 题目名
        [Required]
        public string Name { get; set; }
        // 题目链接
        [Required]
        public string Url { get; set; }
    }
}