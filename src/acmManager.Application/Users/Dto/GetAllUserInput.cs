using System.ComponentModel.DataAnnotations;

namespace acmManager.Users.Dto
{
    public class GetAllUserInput
    {
        public UserInfoDto Filter { get; set; }
        
        [Required]
        public int MaxResultCount { get; set; }
        [Required]
        public int SkipCount { get; set; }
    }
}