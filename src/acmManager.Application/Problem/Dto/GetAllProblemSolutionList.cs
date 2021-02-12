using System;
using System.Collections.Generic;

namespace acmManager.Problem.Dto
{
    public class GetAllProblemSolutionList
    {
        public long Id { get; set; }
        
        public string ProblemName { get; set; }
        public string ProblemUrl { get; set; }
        
        public string ArticleTitle { get; set; }
        public long CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public IEnumerable<ProblemTypeDto> ProblemTypes { get; set; }
    }
}