using Abp.AutoMapper;

namespace acmManager.Article.Dto
{
    [AutoMapTo(typeof(Article))]
    public class UpdateArticleInput : CreateArticleInput
    {
        public long Id { get; set; }
    }
}