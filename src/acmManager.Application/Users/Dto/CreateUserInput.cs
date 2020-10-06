using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using acmManager.Authorization.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace acmManager.Users.Dto
{
    [AutoMapTo(typeof(UserInfo))]
    public class CreateUserInput
    {
        // 学号
        [Required(AllowEmptyStrings = false)]
        public string StudentNumber { get; set; }
        // 学院
        public string Org { get; set; }
        // 电话
        public string Mobile { get; set; }
        // 性别
        [EnumDataType(typeof(UserGender))]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserGender? Gender { get; set; }
        // 专业
        public string Major { get; set; }
        // 班号
        public string ClassId { get; set; }
        // 校区
        public string Location { get; set; }
        // 学生类型: 本科生? 研究生?
        public string StudentType { get; set; }
        // 照片 (二次元头像)
        // public File.File Photo { get; set; }
        // 邮箱
        public string Email { get; set; }
        // 姓名
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        // 类型
        [EnumDataType(typeof(UserType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserType? Type { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(AbpUserBase.MaxPasswordLength)]
        public string Password { get; set; }
    }
}