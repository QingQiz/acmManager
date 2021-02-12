using System.Collections.Generic;
using acmManager.Article.Dto;

namespace acmManager.Web.Models.Shared
{
    public class CommentViewModel
    {
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}