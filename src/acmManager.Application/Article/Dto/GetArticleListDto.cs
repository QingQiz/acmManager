using System;

namespace acmManager.Article.Dto
{
    public class GetArticleListDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
    }
}