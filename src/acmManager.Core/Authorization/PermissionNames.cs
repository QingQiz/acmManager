namespace acmManager.Authorization
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
        public const string PagesUsers_GetOne = "Pages.Users.GetOne";

        public const string PagesUsers_Promote = "Pages.Users.Promote";
        public const string PagesUsers_Relegate = "Pages.Users.Relegate";

        // 进入管理员界面的权限
        public const string PagesUsers_Admin = "Pages.Users.Admin";

        // 一般的证书权限
        public const string PagesUsers_Certificate = "Pages.Users.Certificate";
        // 上传证书的权限
        public const string PagesUsers_Certificate_Upload = "Pages.Users.Certificate.Upload";
        // 删除任意证书的权限
        public const string PagesUsers_Certificate_DeleteAll = "Pages.Users.Certificate.DeleteAll";
        // 获取任意证书的权限
        public const string PagesUsers_Certificate_GetAll = "Pages.Users.Certificate.GetAll";

        // 文章权限
        public const string PagesUsers_Article = "Pages.Users.Article";
        public const string PagesUsers_Article_Create = "Pages.Users.Article.Create";
        public const string PagesUsers_Article_Delete = "Pages.Users.Article.Delete";
        public const string PagesUsers_Article_Update = "Pages.Users.Article.Update";
    }
}
