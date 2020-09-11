using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace acmManager.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";
        
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
        
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
