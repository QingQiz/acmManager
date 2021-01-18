using System.Collections.Generic;

namespace acmManager.Problem.Dto
{
    public class GetSolutionOutput
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public IEnumerable<ProblemTypeDto> Types { get; set; }
        
        public string Content { get; set; }
        
        public long GoodVoteCnt { get; set; }
    }
}