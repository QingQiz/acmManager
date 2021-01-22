namespace acmManager.Article.Dto
{
    public class DeleteCommentInput
    {
        public long ArticleId { get; set; }
        public long CommentId { get; set; }
    }
}