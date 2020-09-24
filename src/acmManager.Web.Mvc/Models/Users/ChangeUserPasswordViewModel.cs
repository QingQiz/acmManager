using System.ComponentModel.DataAnnotations;
using acmManager.Users.Dto;

namespace acmManager.Web.Models.Users
{
    public class ChangeUserPasswordViewModel: ChangePasswordDto
    {
        [Required]
        public string NewPasswordAgain { get; set; }
    }
}