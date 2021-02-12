using System;
namespace acmManager.Web.Models.Shared
{
    public class CreatorViewModel
    {
        public long CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        
        public bool ShowEmail { get; set; }
        public bool ShowCreationTime { get; set; }

        public CreatorViewModel(long creator, bool showEmail = true)
        {
            CreatorId = creator;
            ShowEmail = showEmail;
            ShowCreationTime = false;
        }

        public CreatorViewModel(long creator, DateTime creationTime)
        {
            CreatorId = creator;
            ShowEmail = false;
            ShowCreationTime = true;
            CreationTime = creationTime;
        }
    }
}