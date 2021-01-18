using System.Collections.Generic;

namespace acmManager.Problem.Dto
{
    public class GetAllProblemSolutionList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public long CreatorUserId { get; set; }
        public long GoodVoteCnt { get; set; }
        public IEnumerable<ProblemTypeDto> ProblemTypes { get; set; }
    }
}