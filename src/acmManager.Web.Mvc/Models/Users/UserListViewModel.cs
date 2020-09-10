using System.Collections.Generic;
using acmManager.Roles.Dto;

namespace acmManager.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
