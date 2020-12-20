using System.Collections.Generic;
using acmManager.Contest.Dto;

namespace acmManager.Web.Models.Contest
{
    public class ContestSignUpListViewModel
    {
        public List<GetContestSignUpListOutput> SignUps { get; set; }

        public long ContestId { get; set; }
    }
}