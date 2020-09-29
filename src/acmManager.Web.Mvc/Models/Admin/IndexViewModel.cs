﻿using JetBrains.Annotations;
using GetAllUserViewModel = System.Collections.Generic.IEnumerable<acmManager.Users.Dto.UserDto>;

namespace acmManager.Web.Models.Admin
{
    public class IndexViewModel
    {
        [CanBeNull] public GetAllUserViewModel Users;
        [CanBeNull] public GetAllUserWithFilterViewModel CurrentUserFilter;
    }
}