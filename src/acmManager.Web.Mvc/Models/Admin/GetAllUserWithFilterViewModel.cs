using Abp.AutoMapper;
using acmManager.Users.Dto;

namespace acmManager.Web.Models.Admin
{
    [AutoMapTo(typeof(UserInfoDto))]
    public class GetAllUserWithFilterViewModel : UserInfoDto
    {
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
    }
}