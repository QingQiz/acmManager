namespace acmManager.Web.Models.Shared
{
    public class CommentToViewModel
    {
        public long ArticleId { get; set; }

        public string CommentTitle { get; set; }

        public string CommentToLink { get; set; }

        public long ReplyToCommentId { get; set; }

        public string ContentInit { get; set; } = "Comment here...\n\n\n\n\n";
        
        public string ReturnUrl { get; set; }
    }
}