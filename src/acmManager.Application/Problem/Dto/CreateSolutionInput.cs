using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using acmManager.Article.Dto;

namespace acmManager.Problem.Dto
{
    [AutoMapTo(typeof(Problem), typeof(Article.Article))]
    public class CreateSolutionInput : CreateArticleInput
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Url { get; set; }
        
        [Required]
        public string Description { get; set; }

        public List<long> TypeIds { get; set; }
    }
}