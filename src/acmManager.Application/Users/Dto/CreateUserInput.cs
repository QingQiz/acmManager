using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using acmManager.Authorization.Users;

namespace acmManager.Users.Dto
{
    [AutoMapTo(typeof(UserInfo))]
    public class CreateUserInput: UserInfoDto
    {
        [Required]
        [MaxLength(AbpUserBase.MaxPasswordLength)]
        public string Password { get; set; }
    }
}