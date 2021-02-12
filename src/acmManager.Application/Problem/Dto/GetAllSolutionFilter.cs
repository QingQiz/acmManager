using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace acmManager.Problem.Dto
{
    public class GetAllSolutionFilter
    {
        public long UserId { get; set; } = 0;
        public string KeyWords { get; set; }
        
        public IEnumerable<long> TypeIds { get; set; }
        
        [Required]
        public int MaxResultCount { get; set; }
        
        [Required]
        public int SkipCount { get; set; }
    }
}