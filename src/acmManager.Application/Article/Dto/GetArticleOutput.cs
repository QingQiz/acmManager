using Abp.AutoMapper;

namespace acmManager.Article.Dto
{
    [AutoMapFrom(typeof(Article))]
    public class GetArticleOutput : CreateArticleInput
    {
        public long Id { get; set; }
    }
}