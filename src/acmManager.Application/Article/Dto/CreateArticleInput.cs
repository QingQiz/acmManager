using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace acmManager.Article.Dto
{
    [AutoMapTo(typeof(Article))]
    public class CreateArticleInput
    {
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}