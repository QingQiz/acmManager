﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Authorization.Roles;
using acmManager.Authorization.Users;
using acmManager.Configuration;
using acmManager.Users.Dto;
using Microsoft.AspNetCore.Identity;

namespace acmManager.Users
{
    public class UserAppService : acmManagerAppServiceBase
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserInfoManager _userInfoManager;
        private readonly SettingManager _settingManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager, UserRegistrationManager userRegistrationManager, UserInfoManager userInfoManager, SettingManager settingManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userRegistrationManager = userRegistrationManager;
            _userInfoManager = userInfoManager;
            _settingManager = settingManager;
        }

        #region NotRemoveToService

        /// <summary>
        /// 执行爬虫，从翱翔门户获取信息
        /// </summary>
        /// <param name="username">账户名</param>
        /// <param name="password">密码</param>
        /// <returns>see `UserInfoDto</returns>
        /// <exception cref="UserFriendlyException"></exception>
        [RemoteService(false)]
        public async Task<UserInfoDto> GetUserInfoFromAoxiangAsync(string username, string password)
        {
            // use crawler to get user information
            var crawlerPath = await _settingManager.GetSettingValueAsync(AppSettingNames.CrawlerPath);

            var process = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c python {crawlerPath} -u {username} -p {password}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();

            // 0    1      2      3       4      5     6       7        8        9
            // id, org, mobile, gender, email, name, class, location, major, studentType
            var result = (await process.StandardOutput.ReadToEndAsync()).Split("\r\n");
            
            // 爬虫执行失败
            if (process.ExitCode != 0)
            {
                throw new UserFriendlyException("Login to uis.nwpu.edu.cn failed, wrong username or password or internal server error");
            }

            return new UserInfoDto()
            {
                StudentNumber = result[0],
                Org = result[1],
                Mobile = result[2],
                Gender = result[3] == "男" ? UserGender.Male : UserGender.Female,
                Major = result[8],
                ClassId = result[6],
                Location = result[7],
                StudentType = result[9],
                Email = result[4],
                Name = result[5]
            };
        }

        #endregion

        #region privilegeApi
        
        /// <summary>
        /// 创建一个用户 （特权操作）
        /// </summary>
        /// <param name="input">see `CreateUserInput`</param>
        /// <returns>see `UserInfoDto` </returns>
        [AbpAuthorize(PermissionNames.PagesUsers_Create)]
        public async Task<UserInfoDto> CreateAsync(CreateUserInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(input.Name, input.Name, input.Email,
                input.StudentNumber, input.Password, true);
            var userInfo = ObjectMapper.Map<UserInfo>(input);
            
            await _userInfoManager.Create(userInfo);

            user.UserInfo = userInfo;
            await _userManager.UpdateAsync(user);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<UserInfoDto>(userInfo);
        }
        
        #endregion

        /// <summary>
        /// 重新从翱翔门户获取信息
        /// </summary>
        /// <param name="input">see `UpdateUserInfoInput`</param>
        /// <returns>see `UserInfoDto`</returns>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize]
        public async Task<UserInfoDto> UpdateInfoAsync(UpdateUserInfoInput input)
        {
            // current user
            var user = await GetCurrentUserAsync();

            if (user.UserName.Length != 10)
            {
                // only student can update info
                throw new UserFriendlyException("Current user can not update info");
            }

            // get user info from aoxiang
            var userInfoDto = await GetUserInfoFromAoxiangAsync(user.UserName, input.Password);

            // map new info to old info
            ObjectMapper.Map(userInfoDto, user.UserInfo);

            // update
            await _userInfoManager.Update(user.UserInfo);

            return userInfoDto;
        }

        /*
        [AbpAuthorize]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        [AbpAuthorize]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        [AbpAuthorize]
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        [AbpAuthorize]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }
        */
    }
}

