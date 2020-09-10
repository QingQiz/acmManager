using System.Collections.Generic;
using acmManager.Roles.Dto;

namespace acmManager.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}