using System.ComponentModel.DataAnnotations;

namespace acmManager.Web.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(10)]
        public string AoxiangUsername { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string AoxiangPassword { get; set; }
    }
}
