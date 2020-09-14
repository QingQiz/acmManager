using System.ComponentModel.DataAnnotations;

namespace acmManager.Authorization.Accounts.Dto
{
    public class ResetPasswordDto
    {
        [Required(AllowEmptyStrings = false)]
        public string NewPassword { get; set; }
        
        [Required]
        public long UserId { get; set; }
    }
}