using System;
using Abp.AutoMapper;
using acmManager.File.Dto;
using acmManager.Users.Dto;

namespace acmManager.Web.Models.Users
{
    [AutoMapFrom(typeof(UserDto))]
    public class UserProfileViewModel : UserDto
    {
        public GetFileOutput Photo { get; set; }
        
        public DateTime CreationTime { get; set; }
    }
}