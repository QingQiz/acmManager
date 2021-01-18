using System.Collections.Generic;

namespace acmManager.Problem.Dto
{
    public class GetAllSolutionOutput
    {
        public IEnumerable<GetAllProblemSolutionList> Solutions { get; set; }
        public long AllResultCount { get; set; }
    }
}