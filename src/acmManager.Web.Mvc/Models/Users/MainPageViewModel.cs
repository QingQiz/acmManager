using acmManager.Users.Dto;
using acmManager.Web.Models.Shared;

namespace acmManager.Web.Models.Users
{
    public class MainPageViewModel : ChartViewModel
    {
        public long UserId { get; set; }
        public GetUserInfoDto UserInfo { get; set; }
    }
}