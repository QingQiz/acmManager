using System.Collections.Generic;
using acmManager.Roles.Dto;

namespace acmManager.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
