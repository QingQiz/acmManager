using Abp.AutoMapper;
using acmManager.Authorization.Users;

namespace acmManager.Users.Dto
{
    [AutoMap(typeof(UserInfoDto), typeof(UserInfo))]
    public class UserDto: UserInfoDto
    {
        public long UserId { get; set; }
    }
}
