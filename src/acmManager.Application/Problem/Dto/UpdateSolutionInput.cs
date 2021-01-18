using acmManager.Article.Dto;

namespace acmManager.Problem.Dto
{
    public class UpdateSolutionInput : CreateSolutionInput
    {
        public long Id { get; set; }
    }
}