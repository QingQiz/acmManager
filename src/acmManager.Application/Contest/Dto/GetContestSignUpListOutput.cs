using Abp.AutoMapper;
using acmManager.Authorization.Users;

namespace acmManager.Contest.Dto
{
    [AutoMapFrom(typeof(UserInfo))]
    public class GetContestSignUpListOutput
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Org { get; set; }
        public UserGender Gender { get; set; }
        public string StudentNumber { get; set; }
        public string Password { get; set; }
    }
}