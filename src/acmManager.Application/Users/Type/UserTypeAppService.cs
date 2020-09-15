using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Authorization.Roles;
using acmManager.Authorization.Users;
using acmManager.Users.Type.Dto;

namespace acmManager.Users.Type
{
    public class UserTypeAppService: acmManagerAppServiceBase
    {
        private readonly RoleManager _roleManager;

        public UserTypeAppService(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        #region NotRemoteToService

        [RemoteService(false)]
        public async Task<Role> GetUserRoleByTypeAsync(UserType type)
        {
            switch (type)
            {
                case UserType.Member:
                    return await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Member);

                case UserType.TeamLeader:
                    return await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.TeamLeader);

                case UserType.Teacher:
                case UserType.Administrator:
                    return await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Admin);

                case UserType.TempMember:
                case UserType.RetiredMember:
                    return await _roleManager.GetRoleByNameAsync(StaticRoleNames.Tenants.Default);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        [RemoteService(false)]
        public async Task ChangeUserTypeAsync(User user, UserType newType)
        {
            user.Roles.Clear();

            var role = await GetUserRoleByTypeAsync(newType);
            
            user.Roles.Add(new UserRole(user.TenantId, user.Id, role.Id));

            user.UserInfo.Type = newType;
        }

        [RemoteService(false)]
        public void CheckUserType(User user, UserType shouldBe)
        {
            if (user.UserInfo is null || user.UserInfo.Type != shouldBe)
            {
                throw new UserFriendlyException("the user does not support the operation, the user type should be: " + shouldBe);
            }
        }

        #endregion

        /// <summary>
        /// 正式队员->退役队员
        /// </summary>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize(PermissionNames.PagesUsers_Relegate)]
        public async Task RetireAsync()
        {
            var user = await GetCurrentUserAsync(includeRoles: true);
            
            CheckUserType(user, UserType.Member);
            
            await ChangeUserTypeAsync(user, UserType.RetiredMember);
        }

        /// <summary>
        /// 临时队员->正式队员
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize(PermissionNames.PagesUsers_Promote)]
        public async Task ToMemberAsync(long userId)
        {
            var user = await UserManager.GetUserByIdWithRolesAsync(userId);
            
            CheckUserType(user, UserType.TempMember);

            await ChangeUserTypeAsync(user, UserType.Member);
        }

        /// <summary>
        /// 队长辞职
        /// </summary>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize(PermissionNames.PagesUsers_Relegate)]
        public async Task ResignationAsync()
        {
            var user = await GetCurrentUserAsync(includeRoles: true);
            
            CheckUserType(user, UserType.TeamLeader);

            await ChangeUserTypeAsync(user, UserType.Member);
        }

        /// <summary>
        /// 更新用户类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesUsers_Update)]
        public async Task ChangeUserTypeAsync(UpdateUserTypeDto input)
        {
            var user = await UserManager.GetUserByIdWithRolesAsync(input.UserId);

            await ChangeUserTypeAsync(user, input.UserType);
        }
    }
}