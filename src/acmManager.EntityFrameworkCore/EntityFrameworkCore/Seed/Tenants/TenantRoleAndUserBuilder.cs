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
using acmManager.Problem;

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
            // For team leader
            CreateExtraRoles(StaticRoleNames.Tenants.TeamLeader, false, new List<string>()
            {
                PermissionNames.PagesUsers_Contest,
                PermissionNames.PagesUsers_Contest_SignUp,
                
                PermissionNames.PagesUsers_Article,
                PermissionNames.PagesUsers_Article_Create,
                PermissionNames.PagesUsers_Article_Delete,
                PermissionNames.PagesUsers_Article_Update,
                
                PermissionNames.PagesUsers_Admin,
                
                PermissionNames.PagesUsers_Certificate,
                PermissionNames.PagesUsers_Certificate_Upload,
                
                PermissionNames.PagesUsers_GetAll,
                PermissionNames.PagesUsers_Promote,
                PermissionNames.PagesUsers_Relegate,
                
                PermissionNames.PagesUsers_Problem,
                PermissionNames.PagesUsers_Problem_Delete
            });
            // for member
            CreateExtraRoles(StaticRoleNames.Tenants.Member, isDefault: false, new List<string>()
            {
                PermissionNames.PagesUsers_Contest_SignUp,
                 
                PermissionNames.PagesUsers_Article,
                
                PermissionNames.PagesUsers_Relegate,
                
                PermissionNames.PagesUsers_Certificate,
                PermissionNames.PagesUsers_Certificate_Upload,
                
                PermissionNames.PagesUsers_Problem,
            });
            // for temp user
            CreateExtraRoles(StaticRoleNames.Tenants.Default, isDefault: true, new List<string>()
            {
                PermissionNames.PagesUsers_Contest_SignUp,
                
                PermissionNames.PagesUsers_Article,
                
                PermissionNames.PagesUsers_Problem,
            });
            
            // Create default problem type
            var problemType = _context.ProblemTypes.IgnoreQueryFilters();
            if (!problemType.Any())
            {
                var defaultTypes = new[]
                {
                    new ProblemType {Name = "2-sat", Description = "2-satisfiability"},
                    new ProblemType {Name = "binary search", Description = "Binary search"},
                    new ProblemType {Name = "bitmasks", Description = "Bitmasks"},
                    new ProblemType {Name = "brute force", Description = "Brute force"},
                    new ProblemType {Name = "chinese remainder theorem", Description = "Сhinese remainder theorem"},
                    new ProblemType {Name = "combinatorics", Description = "Combinatorics"},
                    new ProblemType {Name = "constructive algorithms", Description = "Constructive algorithms"},
                    new ProblemType
                    {
                        Name = "data structures",
                        Description = "Heaps, binary search trees, segment trees, hash tables, etc"
                    },
                    new ProblemType {Name = "dfs and similar", Description = "Dfs and similar"},
                    new ProblemType {Name = "divide and conquer", Description = "Divide and Conquer"},
                    new ProblemType {Name = "dp", Description = "Dynamic programming"},
                    new ProblemType {Name = "dsu", Description = "Disjoint set union"},
                    new ProblemType {Name = "expression parsing", Description = "Parsing expression grammar"},
                    new ProblemType {Name = "fft", Description = "Fast Fourier transform"},
                    new ProblemType {Name = "flows", Description = "Graph network flows"},
                    new ProblemType {Name = "games", Description = "Games, Sprague–Grundy theorem"},
                    new ProblemType {Name = "geometry", Description = "Geometry, computational geometry"},
                    new ProblemType
                    {
                        Name = "graph matchings",
                        Description = "Graph matchings, König's theorem, vertex cover of bipartite graph"
                    },
                    new ProblemType {Name = "graphs", Description = "Graphs"},
                    new ProblemType {Name = "greedy", Description = "Greedy algorithms"},
                    new ProblemType {Name = "hashing", Description = "Hashing, hashtables"},
                    new ProblemType
                    {
                        Name = "implementation",
                        Description = "Implementation problems, programming technics, simulation"
                    },
                    new ProblemType {Name = "interactive", Description = "Interactive problem"},
                    new ProblemType
                        {Name = "math", Description = "Mathematics including integration, differential equations, etc"},
                    new ProblemType
                    {
                        Name = "matrices",
                        Description = "Matrix multiplication, determinant, Cramer's rule, systems of linear equations"
                    },
                    new ProblemType {Name = "meet-in-the-middle", Description = "Meet-in-the-middle"},
                    new ProblemType
                        {Name = "number theory", Description = "Number theory: Euler function, GCD, divisibility, etc"},
                    new ProblemType
                    {
                        Name = "probabilities",
                        Description = "Probabilities, expected values, statistics, random variables, etc"
                    },
                    new ProblemType {Name = "schedules", Description = "Scheduling Algorithms"},
                    new ProblemType
                        {Name = "shortest paths", Description = "Shortest paths on weighted and unweighted graphs"},
                    new ProblemType {Name = "sortings", Description = "Sortings, orderings"},
                    new ProblemType
                    {
                        Name = "string suffix structures",
                        Description = "Suffix arrays, suffix trees, suffix automatas, etc"
                    },
                    new ProblemType
                    {
                        Name = "strings",
                        Description = "Prefix- and Z-functions, suffix structures, Knuth–Morris–Pratt algorithm, etc"
                    },
                    new ProblemType {Name = "ternary search", Description = "Ternary search"},
                    new ProblemType {Name = "trees", Description = "Trees"},
                    new ProblemType {Name = "two pointers", Description = "Two pointers"},
                };
                _context.ProblemTypes.AddRange(defaultTypes);
            }
        }
    }
}
