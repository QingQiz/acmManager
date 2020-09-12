namespace acmManager.Authorization.Users
{
    public enum UserType
    {
        // 临时队员
        TempMember,
        // 退役队员
        RetiredMember,
        // 正式队员
        Member,
        // 队长 
        TeamLeader,
        // 教师
        Teacher,
        // 管理员
        Administrator,
    }
}