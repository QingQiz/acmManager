using System;
using System.Collections.Generic;
using Abp.AutoMapper;

namespace acmManager.Article.Dto
{
    [AutoMapFrom(typeof(Article))]
    public class GetArticleOutput : CreateArticleInput
    {
        /// <summary>
        /// blog id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// article id
        /// </summary>
        public long ArticleId { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
        
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
    }
}