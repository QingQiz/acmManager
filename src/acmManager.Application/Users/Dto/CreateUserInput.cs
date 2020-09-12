using System.ComponentModel.DataAnnotations;

namespace acmManager.Users.Dto
{
    public class CreateUserInput
    {
        [Required]
        [StringLength(10)]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}