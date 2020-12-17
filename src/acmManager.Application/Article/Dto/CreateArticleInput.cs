using Abp.AutoMapper;

namespace acmManager.Article.Dto
{
    [AutoMapTo(typeof(Article))]
    public class CreateArticleInput
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}