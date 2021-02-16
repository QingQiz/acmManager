namespace acmManager.Article.Dto
{
    public class GetArticleListFilter
    {
        public long UserId { get; set; }
        public string Keyword { get; set; }
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
    }
}