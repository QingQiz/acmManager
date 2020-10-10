using System.Collections.Generic;
using Abp.AutoMapper;
using acmManager.Certificate.Dto;
using acmManager.Users.Dto;

namespace acmManager.Web.Models.Admin
{
    [AutoMapFrom(typeof(GetUserInfoDto), typeof(UserDto))]
    public class UserProfileViewModel : UserDto
    {
        public IEnumerable<GetCertificateOutput> Certificate { get; set; }
    }
}