using System.ComponentModel.DataAnnotations;

namespace acmManager.Web.Models.Users
{
    public class UpdateFromAoxiangViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PasswordAgain { get; set; }
    }
}