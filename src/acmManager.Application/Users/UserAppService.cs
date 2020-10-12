using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Authorization.Accounts;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Configuration;
using acmManager.Users.Dto;
using acmManager.Users.Type;
using Microsoft.AspNetCore.Identity;

namespace acmManager.Users
{
    public class UserAppService : acmManagerAppServiceBase
    {
        private readonly UserManager _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly LogInManager _logInManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserInfoManager _userInfoManager;
        private readonly SettingManager _settingManager;
        private readonly UserTypeAppService _userTypeAppService;

        public UserAppService(UserManager userManager,
            IPasswordHasher<User> passwordHasher,
            LogInManager logInManager, UserRegistrationManager userRegistrationManager, UserInfoManager userInfoManager, SettingManager settingManager, UserTypeAppService userTypeAppService)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _logInManager = logInManager;
            _userRegistrationManager = userRegistrationManager;
            _userInfoManager = userInfoManager;
            _settingManager = settingManager;
            _userTypeAppService = userTypeAppService;
        }

        #region NotRemoteToService

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

        /// <summary>
        /// 将 User 转化为 UserDto
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <returns><see cref="UserDto"/></returns>
        [RemoteService(false)]
        public UserDto UserToDto(User user)
        {
            var ret = user.UserInfo == null ? new UserDto() : ObjectMapper.Map<UserDto>(user.UserInfo);
            ret.UserId = user.Id;
            return ret;
        }

        /// <summary>
        /// 将 User 转化为 UserInfoDto
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <returns><see cref="UserInfoDto"/></returns>
        [RemoteService(false)]
        public UserInfoDto UserToInfoDto(User user)
        {
            return ObjectMapper.Map<UserInfoDto>(user.UserInfo) ?? new UserInfoDto();
        }
        
        #endregion

        #region privilegeApi

        /// <summary>
        /// 创建一个用户 （特权操作）
        /// </summary>
        /// <param name="input">see `CreateUserInput`</param>
        /// <returns>see `UserDto` </returns>
        [AbpAuthorize(PermissionNames.PagesUsers_Create)]
        public async Task<UserDto> CreateAsync(CreateUserInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(input.Name, input.Name, input.Email ?? "",
                input.StudentNumber, input.Password, true);
            
            var userInfo = ObjectMapper.Map<UserInfo>(input);
            user.UserInfo = userInfo;

            // change user role and user type
            await _userTypeAppService.ChangeUserTypeAsync(user, userInfo.Type);

            return UserToDto(user);
        }

        /// <summary>
        /// 更新用户信息，特权接口
        /// </summary>
        /// <param name="input">see `UserDto`</param>
        /// <returns>see `UserDto`</returns>
        [AbpAuthorize(PermissionNames.PagesUsers_Update)]
        public async Task<UserDto> UpdateAsync(UserDto input)
        {
            var user = await UserManager.GetUserByIdWithRolesAsync(input.UserId);
            var oldType = user.UserInfo.Type;
            ObjectMapper.Map(input, user.UserInfo);
            var newType = user.UserInfo.Type;

            if (newType !=  oldType)
            {
                await _userTypeAppService.ChangeUserTypeAsync(user, newType);
            }
            
            return UserToDto(user);
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesUsers_Delete)]
        public async Task DeleteAsync(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id);
            var userInfo = user.UserInfo;
            await _userManager.DeleteAsync(user);
            await _userInfoManager.Delete(userInfo.Id);
        }
        
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="input"><see cref="ResetPasswordDto"/></param>
        [AbpAuthorize(PermissionNames.PagesUsers_Update)]
        public async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Get user info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="UserDto"/></returns>
        [AbpAuthorize(PermissionNames.PagesUsers_GetOne)]
        public async Task<UserDto> GetAsync(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            return UserToDto(user);
        }

        /// <summary>
        /// 获取所有用户的信息，筛选+分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.PagesUsers_GetAll)]
        public async Task<IEnumerable<UserDto>> GetAllUserAsync(GetAllUserInput input)
        {
            if (input.Filter == null)
            {
                return await Task.Run(() =>
                    _userManager.Query().Skip(input.SkipCount).Take(input.MaxResultCount).ToList().Select(UserToDto));
            }

            var empty = new Func<string, bool>(string.IsNullOrEmpty);
            var substr = new Func<string, string, bool>((s1, s2) => s1 != null && s1.Contains(s2));

            return await Task.Run(() => _userManager
                .Query()
                .Where(u => u.UserInfo != null)
                .WhereIf(!empty(input.Filter.StudentNumber),
                    u => substr(u.UserInfo.StudentNumber, input.Filter.StudentNumber))
                .WhereIf(!empty(input.Filter.Org), 
                    u => substr(u.UserInfo.Org, input.Filter.Org))
                .WhereIf(!empty(input.Filter.Mobile),
                    u => substr(u.UserInfo.Mobile, input.Filter.Mobile))
                .WhereIf(!empty(input.Filter.Major), 
                    u => substr(u.UserInfo.Major, input.Filter.Major))
                .WhereIf(!empty(input.Filter.ClassId),
                    u => substr(u.UserInfo.ClassId, input.Filter.ClassId))
                .WhereIf(!empty(input.Filter.Location),
                    u => substr(u.UserInfo.Location, input.Filter.Location))
                .WhereIf(!empty(input.Filter.StudentType),
                    u => substr(u.UserInfo.StudentType, input.Filter.StudentType))
                .WhereIf(!empty(input.Filter.Email), 
                    u => substr(u.UserInfo.Email, input.Filter.Email))
                .WhereIf(!empty(input.Filter.Name), 
                    u => substr(u.UserInfo.Name, input.Filter.Name))
                .WhereIf(input.Filter.Gender != null, 
                    u => u.UserInfo.Gender == input.Filter.Gender)
                .WhereIf(input.Filter.Type != null, 
                    u => u.UserInfo.Type == input.Filter.Type)
                .Skip(input.SkipCount).Take(input.MaxResultCount).ToList().Select(UserToDto));
        }
        
        #endregion

        #region UserInfo

        /// <summary>
        /// 从翱翔门户更新用户资料
        /// </summary>
        /// <param name="input">see `UpdateUserInfoInput`</param>
        /// <returns>see `UserInfoDto`</returns>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize]
        public async Task<UserInfoDto> UpdateInfoFromAoxiangAsync(UpdateUserInfoFromAoxiangInput input)
        {
            // current user
            var user = await GetCurrentUserAsync();

            if (user.UserName.Length != 10)
            {
                // only student can update info
                throw new UserFriendlyException("Current user can not update info");
            }

            // get user info from aoxiang
            var userType = user.UserInfo.Type;
            var userInfoDto = await GetUserInfoFromAoxiangAsync(user.UserName, input.Password);

            // map new info to old info
            ObjectMapper.Map(userInfoDto, user.UserInfo);
            user.UserInfo.Type = userType;
            
            return userInfoDto;
        }

        /// <summary>
        /// 用户更改自己的资料
        /// </summary>
        /// <param name="input">see `UpdateUserInfoInput`</param>
        /// <returns>see `UserInfoDto`</returns>
        [AbpAuthorize]
        public async Task<UserInfoDto> UpdateInfoAsync(UpdateUserInfoInput input)
        {
            input.Email ??= "";
            input.Mobile ??= "";

            var user = await GetCurrentUserAsync();

            if (!(new Regex(
                    // From https://regexlib.com/RETester.aspx?regexp_id=26
                    @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .IsMatch(input.Email)))
            {
                throw new UserFriendlyException("Invalid email address: " + input.Email);
            }

            if (!(new Regex(@"^\+?[0-9]{0,15}$")).IsMatch(input.Mobile))
            {
                throw new UserFriendlyException("Invalid mobile: " + input.Mobile);
            }
  
            user.UserInfo.Email = input.Email;
            user.UserInfo.Mobile = input.Mobile;

            return ObjectMapper.Map<UserInfoDto>(user.UserInfo);
        }

        /// <summary>
        /// 获取当前用户的信息
        /// </summary>
        /// <returns><see cref="UserInfoDto"/></returns>
        [UnitOfWork]
        [AbpAuthorize]
        public virtual async Task<UserDto> GetMeAsync()
        {
            var user = await GetCurrentUserAsync();

            return user.UserInfo == null ? null : UserToDto(user);
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="input"><see cref="ChangePasswordDto"/></param>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize]
        public async Task ChangePasswordAsync(ChangePasswordDto input)
        {
            var user = await GetCurrentUserAsync();
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record. Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must between 4 and 28 characters.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="GetUserInfoDto"/></returns>
        [UnitOfWork]
        [AbpAuthorize]
        public virtual async Task<GetUserInfoDto> GetUserInfoAsync(long userId)
        {
            var user = await UserManager.GetUserByIdAsync(userId);

            return ObjectMapper.Map<GetUserInfoDto>(user.UserInfo);
        }

        #endregion
    }
}

