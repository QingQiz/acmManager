﻿namespace acmManager.Authorization
{
    public static class PermissionNames
    {
        public const string Pages = "Pages";
        
        public const string PagesTenants = "Pages.Tenants";

        public const string PagesUsers = "Pages.Users";

        public const string PagesRoles = "Pages.Roles";

        // put permission name here
        public const string PagesUsers_Create = "Pages.Users.Create";
        public const string PagesUsers_Update = "Pages.Users.Update";
        public const string PagesUsers_Delete = "Pages.Users.Delete";
        public const string PagesUsers_GetAll = "Pages.Users.GetAll";

        public const string PagesUsers_Promote = "Pages.Users.Promote";
        public const string PagesUsers_Relegate = "Pages.Users.Relegate";

        public const string PagesUsers_Admin = "Pages.Users.Admin";
    }
}
