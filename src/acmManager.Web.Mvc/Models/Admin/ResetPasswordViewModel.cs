using System.ComponentModel.DataAnnotations;

namespace acmManager.Web.Models.Admin
{
    public class ResetPasswordViewModel
    {
        public long UserId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NewPasswordAgain { get; set; }
    }
}