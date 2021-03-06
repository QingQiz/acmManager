using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Users;
using acmManager.Users.Dto;
using acmManager.Utils;
using Castle.Core;

namespace acmManager.Authorization.Accounts
{
    public class AccountAppService : acmManagerAppServiceBase
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        //public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";
        public const string PasswordRegex = "^.{4,28}$";

        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;
        private readonly UserInfoManager _userInfoManager;
        private readonly UserAppService _userAppService;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager, UserManager userManager, UserInfoManager userInfoManager, UserAppService userAppService)
        {
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
            _userInfoManager = userInfoManager;
            _userAppService = userAppService;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }


        /// <summary>
        /// 用户注册，使用翱翔门户的账户名和密码创建一个账户
        /// </summary>
        /// <param name="input">见 `CreateUserInput` </param>
        /// <returns>返回用户信息，见 `UserInfoDto`</returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<Pair<long, UserInfoDto>> Register(RegisterInput input)
        {
            if (_userManager.Query().Any(u => u.UserInfo != null && u.UserInfo.StudentNumber == input.Username))
            {
                throw new UserFriendlyException("User Exists");
            }

            var userInfoDto = await new Crawler(input.Username, input.Password).GetUserInfo();
            
            // true means: Assumed email address is always confirmed.
            var email = userInfoDto.StudentNumber + "@temp.mail.com";
            var user = await _userRegistrationManager.RegisterAsync(
                userInfoDto.Name,
                "", 
                email, 
                input.Username,
                input.Password, 
                true);
            
            // Create UserInfo
            var userInfo = ObjectMapper.Map<UserInfo>(userInfoDto);
            await _userInfoManager.Create(userInfo);
            
            // disable lock
            user.IsLockoutEnabled = false;
            // update userinfo
            user.UserInfo = userInfo;
            await _userManager.UpdateAsync(user);

            await CurrentUnitOfWork.SaveChangesAsync();

            // return DTO
            return new Pair<long, UserInfoDto>(user.Id, userInfoDto);
        }
    }
}
