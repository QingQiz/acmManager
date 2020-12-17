using System;

namespace acmManager.Contest.Dto
{
    public class CreateContestInput
    {
        public string Name { get; set; }
        
        public string DescriptionTitle { get; set; }
        public string DescriptionContent { get; set; }
        
        public DateTime SignUpStartTime { get; set; }
        public DateTime SignUpEndTime { get; set; }
    }
}