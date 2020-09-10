using System.ComponentModel.DataAnnotations;

namespace acmManager.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}