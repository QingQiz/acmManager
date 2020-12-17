using acmManager.Article.Dto;

namespace acmManager.Contest.Dto
{
    public class GetContestOutput : GetContestListOutput
    {
        public GetArticleOutput Description { get; set; }
        public GetArticleOutput Result { get; set; }

        public ContestSignUpInfo SignUpInfo { get; set; }
    }
}