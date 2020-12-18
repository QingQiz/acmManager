using acmManager.Users.Dto;

namespace acmManager.Web.Models.Admin
{
    public class IndexViewModel
    {
        public GetAllUserOutput Users;
        public GetAllUserWithFilterViewModel CurrentUserFilter;
    }
}