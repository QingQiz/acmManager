namespace acmManager.Article.Dto
{
    public class CreateCommentInput
    {
        public string Content { get; set; }
        public long ReplyToCommentId { get; set; }
    }
}