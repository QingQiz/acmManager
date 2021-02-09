namespace acmManager.Users.Dto
{
    // 这是非特权用户更改自己资料的输入
    public class UpdateUserInfoInput
    {
        public string Major { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public string ClassId { get; set; }

        public string Location { get; set; }
    }
}