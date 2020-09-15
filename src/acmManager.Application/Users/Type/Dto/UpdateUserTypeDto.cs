using acmManager.Authorization.Users;

namespace acmManager.Users.Type.Dto
{
    public class UpdateUserTypeDto
    {
        public long UserId { get; set; }
        public UserType UserType { get; set; }
    }
}