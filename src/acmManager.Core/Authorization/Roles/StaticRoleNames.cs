namespace acmManager.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";
            public const string TeamLeader = "TeamLeader";
            public const string Member = "Member";
            public const string Default = "Default";
        }
    }
}
