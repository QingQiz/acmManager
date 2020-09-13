using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace acmManager.Users.Dto
{
    public class UpdateUserInfoInput
    {
        [Required]
        [MaxLength(AbpUserBase.MaxPasswordLength)]
        public string Password { get; set; }
    }
}