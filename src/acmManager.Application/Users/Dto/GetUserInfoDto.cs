using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using acmManager.Authorization.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace acmManager.Users.Dto
{
    [AutoMapFrom(typeof(UserInfo))]
    public class GetUserInfoDto
    {
        // 姓名
        public string Name { get; set; }
        // 性别
        [EnumDataType(typeof(UserGender))]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserGender Gender { get; set; }
        // 邮箱
        public string Email { get; set; }
        // 学院
        public string Org { get; set; }
        // 类型
        [EnumDataType(typeof(UserType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserType Type { get; set; }
    }
}