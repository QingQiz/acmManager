using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using acmManager.Authorization;
using acmManager.Authorization.Roles;
using acmManager.Authorization.Users;

namespace acmManager.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly acmManagerDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(acmManagerDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateExtraRoles(string roleName, bool isDefault, ICollection<string> permissions) 
        {
            var role = _context.Roles.IgnoreQueryFilters()
                .FirstOrDefault(r => r.TenantId == _tenantId && r.Name == roleName);

            if (role == null)
            {
                role = _context.Roles.Add(new Role(_tenantId, roleName, roleName)
                    {IsStatic = true, IsDefault = isDefault}).Entity;
                _context.SaveChanges();
            }
            if (!permissions.Any()) return;
            
            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == role.Id)
                .Select(p => p.Name)
                .ToList();

            var toGrant = PermissionFinder.GetAllPermissions(new acmManagerAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant)
                                   && permissions.Contains(p.Name)
                                   && !grantedPermissions.Contains(p.Name))
                .ToList();

            if (!toGrant.Any()) return;
            
            _context.Permissions.AddRange(
                toGrant.Select(p => new RolePermissionSetting()
                {
                    TenantId = _tenantId,
                    Name = p.Name,
                    IsGranted = true,
                    RoleId = role.Id
                })
            );
            _context.SaveChanges();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new acmManagerAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().Include(u => u.UserInfo)
                .FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "sofeeys@outlook.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                adminUser.UserInfo = null;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }

            if (adminUser.UserInfo == null)
            {
                adminUser.UserInfo = new UserInfo();
                _context.SaveChanges();
            }
            
            // Extra Roles
            CreateExtraRoles(StaticRoleNames.Tenants.TeamLeader, false, new List<string>()
            {
                PermissionNames.PagesUsers_Promote,
                PermissionNames.PagesUsers_Relegate,
                PermissionNames.PagesUsers_Admin,
                PermissionNames.PagesUsers_GetAll,
                PermissionNames.PagesUsers_Certificate_Upload,
            });
            CreateExtraRoles(StaticRoleNames.Tenants.Member, isDefault: false, new List<string>()
            {
                PermissionNames.PagesUsers_Relegate,
                PermissionNames.PagesUsers_Certificate_Upload,
            });
            CreateExtraRoles(StaticRoleNames.Tenants.Default, isDefault: true, new List<string>());
        }
    }
}
