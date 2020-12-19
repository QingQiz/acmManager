using System;

namespace acmManager.Contest.Dto
{
    public class UpdateContestInput : CreateContestInput
    {
        public long Id { get; set; }
    }
}