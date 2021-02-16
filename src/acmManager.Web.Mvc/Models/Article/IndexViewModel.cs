using acmManager.Article.Dto;

namespace acmManager.Web.Models.Article
{
    public class IndexViewModel
    {
        public GetArticleListFilter Filter { get; set; }
        public GetArticleListOutput Articles { get; set; }
    }
}