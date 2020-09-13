using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace acmManager.Users.Dto
{
    public class UpdateUserInfoFromAoxiangInput
    {
        [Required]
        [MaxLength(AbpUserBase.MaxPasswordLength)]
        public string Password { get; set; }
    }
}