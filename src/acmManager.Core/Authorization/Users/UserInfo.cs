using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace acmManager.Authorization.Users
{
    [Table("acmMgr.UserInfo")]
    public class UserInfo: Entity<long>
    {
        // 学号
        public string StudentNumber { get; set; }
        // 学院
        public string Org { get; set; }
        // 电话
        public string Mobile { get; set; }
        // 性别
        public UserGender Gender { get; set; }
        // 专业
        public string Major { get; set; }
        // 班号
        public string ClassId { get; set; }
        // 校区
        public string Location { get; set; }
        // 学生类型: 本科生? 研究生?
        public string StudentType { get; set; }
        // 照片 (二次元头像)
        public File.File Photo { get; set; }
        // 邮箱
        public string Email { get; set; }
        // 姓名
        public string Name { get; set; }
    }
}