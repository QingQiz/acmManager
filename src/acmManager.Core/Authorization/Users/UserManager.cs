using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.UI;
using acmManager.Authorization.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace acmManager.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        public UserManager(
            RoleManager roleManager,
            UserStore store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<User>> logger, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings, 
            ISettingManager settingManager)
            : base(
                roleManager, 
                store, 
                optionsAccessor, 
                passwordHasher, 
                userValidators, 
                passwordValidators, 
                keyNormalizer, 
                errors, 
                services, 
                logger, 
                permissionManager, 
                unitOfWorkManager, 
                cacheManager,
                organizationUnitRepository, 
                userOrganizationUnitRepository, 
                organizationUnitSettings, 
                settingManager)
        {
        }

        public IEnumerable<User> Query()
        {
            return Users.Include(u => u.UserInfo);
        }

        public IIncludableQueryable<User, UserInfo> MakeQueryById(long userId)
        {
            var res = Users.Where(u => u.Id == userId);

            if (!res.Any())
            {
                throw new UserFriendlyException("User not found");
            }

            return res.Include(u => u.UserInfo);
        }

        public override async Task<User> GetUserByIdAsync(long userId)
        {
            return await MakeQueryById(userId).FirstAsync();
        }

        public async Task<User> GetUserByIdWithRolesAsync(long userId)
        {
            return await MakeQueryById(userId)
                .Include(u => u.Roles)
                .FirstAsync();
        }

        [UnitOfWork]
        public virtual async Task<User> GetUserByIdWithPhotoAsync(long userId)
        {
            return await MakeQueryById(userId)
                .ThenInclude(info => info.Photo)
                .FirstAsync();
        }

        public async Task<User> GetUserByStudentNumber(string sid)
        {
            var query = await Users
                .Include(u => u.UserInfo)
                .Where(u => u.UserInfo.StudentNumber == sid)
                .ToListAsync();
            
            if (query.Any())
            {
                return query.First();
            }
            throw new UserFriendlyException("No such user: " + sid);
        }
    }
}
