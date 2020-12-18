using System.Collections.Generic;
using acmManager.Contest.Dto;

namespace acmManager.Web.Models.Contest
{
    public class ContestListViewModel
    {
        public List<GetContestListOutput> Contests { get; set; }

        public ContestListViewModel(List<GetContestListOutput> input)
        {
            Contests = input;
        }
    }
}