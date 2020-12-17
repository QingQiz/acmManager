using System;

namespace acmManager.Contest.Dto
{
    public class UpdateContestInput
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime SignUpStartTime { get; set; }
        public DateTime SignUpEndTime { get; set; }
    }
}