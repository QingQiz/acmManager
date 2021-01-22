using System.Collections.Generic;
using acmManager.Article.Dto;

namespace acmManager.Problem.Dto
{
    public class GetSolutionOutput
    {
        // ProblemSolutionId
        public long Id { get; set; }
        
        public long ProblemId { get; set; }
        public string ProblemName { get; set; }
        public string ProblemUrl { get; set; }
        public string ProblemDescription { get; set; }
        public IEnumerable<ProblemTypeDto> ProblemTypes { get; set; }
        
        // Solution Article Id
        public long SolutionId { get; set; }
        public string SolutionTitle { get; set; }
        public string SolutionContent { get; set; }
        
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}