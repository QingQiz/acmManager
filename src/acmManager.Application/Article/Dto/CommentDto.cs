using System;
using Abp.AutoMapper;

namespace acmManager.Article.Dto
{
    [AutoMapFrom(typeof(Comment))]
    public class CommentDto : CreateCommentInput
    {
        public long Id { get; set; }
        
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
    }
}