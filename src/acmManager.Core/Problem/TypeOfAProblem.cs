using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace acmManager.Problem
{
    public class TypeOfAProblem: Entity<long>
    {
        [ForeignKey("Problem")]
        public long ProblemId { get; set; }
        public Problem Problem { get; set; }
        
        [ForeignKey("Type")]
        public long TypeId { get; set; }
        public ProblemType Type { get; set; }
    }
}