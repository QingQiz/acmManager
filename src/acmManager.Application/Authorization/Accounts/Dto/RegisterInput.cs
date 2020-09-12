using System.ComponentModel.DataAnnotations;

namespace acmManager.Authorization.Accounts.Dto
{
    public class RegisterInput
    {
        [Required]
        [StringLength(10)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
