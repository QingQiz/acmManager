using acmManager.Web.Models.Article;

namespace acmManager.Web.Models.Shared
{
    public class ArticleViewModel : IndexViewModel
    {
        public bool Search { get; set; }
        public bool ShowImage { get; set; } = true;
    }
}