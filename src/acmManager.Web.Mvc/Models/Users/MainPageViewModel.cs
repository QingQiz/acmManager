using System.Collections.Generic;
using acmManager.Certificate.Dto;
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
        
        public IEnumerable<GetAllCertificateSummary> CertificateSummaries { get; set; }
    }
}