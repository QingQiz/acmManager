using System;
using Abp.AutoMapper;

namespace acmManager.Contest.Dto
{
    [AutoMapFrom(typeof(Contest))]
    public class GetContestListOutput
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime SignUpStartTime { get; set; }
        public DateTime SignUpEndTime { get; set; }
    }
}