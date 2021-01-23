using Abp.AutoMapper;

namespace acmManager.Problem.Dto
{
    [AutoMapTo(typeof(ProblemType))]
    [AutoMapFrom(typeof(ProblemType))]
    public class ProblemTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}