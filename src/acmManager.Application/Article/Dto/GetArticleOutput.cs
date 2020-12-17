using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;

namespace acmManager.Article.Dto
{
    [AutoMapFrom(typeof(Article))]
    public class GetArticleOutput : CreateArticleInput
    {
    }
}