using System.Threading.Tasks;
using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;
using acmManager.Authorization.Accounts.Dto;
using acmManager.Authorization.Users;
using acmManager.Configuration;
using acmManager.Users.Dto;

namespace acmManager.Authorization.Accounts
{
    public class AccountAppService : acmManagerAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;
        private readonly SettingManager _settingManager;
        private readonly UserInfoManager _userInfoManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager, SettingManager settingManager, UserManager userManager, UserInfoManager userInfoManager)
        {
            _userRegistrationManager = userRegistrationManager;
            _settingManager = settingManager;
            _userManager = userManager;
            _userInfoManager = userInfoManager;
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
        public async Task<UserInfoDto> Register(RegisterInput input)
        {
            // use crawler to get user information
            var crawlerPath = await _settingManager.GetSettingValueAsync(AppSettingNames.CrawlerPath);

            var process = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c python {crawlerPath} -u {input.Username} -p {input.Password}",
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

            // true means: Assumed email address is always confirmed.
            var user = await _userRegistrationManager.RegisterAsync(result[5], "", result[4], result[0],
                input.Password, true);
            
            // Create UserInfo
            var userInfo = new UserInfo()
            {
                StudentNumber = result[0],
                Org = result[1],
                Mobile = result[2],
                Gender = result[3] == "男" ? UserGender.Male : UserGender.Female,
                Major = result[8],
                ClassId = result[6],
                Location = result[7],
                StudentType = result[9],
                Photo = null,
                Email = result[4],
                Name = result[5]
            };
            await _userInfoManager.Create(userInfo);
            
            // update userinfo
            user.UserInfo = userInfo;
            await _userManager.UpdateAsync(user);

            await CurrentUnitOfWork.SaveChangesAsync();

            // return DTO
            return ObjectMapper.Map<UserInfoDto>(userInfo);
        }
    }
}
