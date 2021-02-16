using System.Collections.Generic;

namespace acmManager.Article.Dto
{
    public class GetArticleListOutput
    {
        public IEnumerable<GetArticleListDto> Articles { get; set; }
        public int AllResultCount { get; set; }
    }
}