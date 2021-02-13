using System.Collections.Generic;
using acmManager.Problem.Dto;
using acmManager.Users.Dto;

namespace acmManager.Web.Models.Users
{
    public class MainPageViewModel
    {
        public long UserId { get; set; }
        public GetUserInfoDto UserInfo { get; set; }
        
        public IEnumerable<ProblemTypeDto> ProblemTypes { get; set; }
        
        public IEnumerable<GetAllProblemSolutionList> ProblemSolutions { get; set; }
    }
}