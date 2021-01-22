namespace acmManager.Article.Dto
{
    public class CommentToArticleInput : CreateCommentInput
    {
        public long ArticleId { get; set; }
    }
}