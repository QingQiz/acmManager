using acmManager.Article.Dto;

namespace acmManager.Contest.Dto
{
    public class SetContestResultInput : CreateArticleInput
    {
        public long Id { get; set; }
        
        public new string Content { get; set; }
    }
}