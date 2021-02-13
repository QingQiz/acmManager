using System;

namespace acmManager.Article.Dto
{
    public class CommentDto : CreateCommentInput
    {
        public long Id { get; set; }
        
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
    }
}