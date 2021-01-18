using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace acmManager.Problem.Dto
{
    public class GetAllSolutionFilter
    {
        public string KeyWords { get; set; }
        
        public DateTime? TimeAfter { get; set; }

        public IEnumerable<long> TypeIds { get; set; }
        
        [Required]
        public int MaxResultCount { get; set; }
        
        [Required]
        public int SkipCount { get; set; }
    }
}