using Abp.AutoMapper;
using acmManager.Users.Dto;

namespace acmManager.Web.Models.Admin
{
    [AutoMapFrom(typeof(GetUserInfoDto), typeof(UserDto))]
    public class UserProfileViewModel : UserDto
    {
    }
}