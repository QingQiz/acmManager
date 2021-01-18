using System.Collections.Generic;
using Abp.AutoMapper;
using acmManager.Article.Dto;

namespace acmManager.Problem.Dto
{
    [AutoMapTo(typeof(Problem), typeof(Article.Article))]
    public class CreateSolutionInput : CreateArticleInput
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<long> TypeIds { get; set; }
    }
}