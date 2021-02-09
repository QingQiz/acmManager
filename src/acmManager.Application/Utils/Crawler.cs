using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.UI;
using acmManager.Authorization.Users;
using acmManager.Users.Dto;
using Flurl.Http;

namespace acmManager.Utils
{
    public class Crawler : acmManagerAppServiceBase
    {
        private const string LoginUrlBase = "https://uis.nwpu.edu.cn/";
        private const string LoginUrl = LoginUrlBase + "/cas/login";

        private const string ECampusUrlBase = "https://ecampus.nwpu.edu.cn/";
        private const string ECampusUrl = ECampusUrlBase + "portal-web/html/index.html";

        private const string UserInfoApiUrl =
            "https://ecampus.nwpu.edu.cn/portal-web/api/rest/portalUser/selectUserInfoByCurrentUser";

        private string _username;
        private string _password;
        private CookieSession _sessionLogin;
        private CookieSession _sessionECampus;

        public Crawler(string username, string password)
        {
            _username = username;
            _password = password;

            _sessionLogin = new CookieSession(LoginUrlBase);
            _sessionECampus = new CookieSession(ECampusUrlBase);

        }

        public async Task LoginToUis()
        {
            await _sessionLogin.Request(LoginUrl).WithFakeAgent().GetAsync();
            try
            {
                await _sessionLogin.Request(LoginUrl).WithFakeAgent().WithAutoRedirect(false).PostUrlEncodedAsync(new
                {
                    username = _username,
                    password = _password,
                    currentMenu = 1,
                    execution = "e1s1",
                    _eventId = "submit",
                });
            }
            catch (FlurlHttpException e)
            {
                Logger.Warn("Auth failed: " + e.Message);
                throw new UserFriendlyException("Authentication failed: wrong username or password");
            }
        }

        public async Task LoginToECampus(int retry = 3)
        {
            try
            {
                // login to https://ecampus.nwpu.edu.cn
                await _sessionECampus
                    .Request(ECampusUrl)
                    .WithFakeAgent()
                    .WithAutoRedirect(false)
                    .GetAsync()
                    .RedirectWithSession(_sessionLogin)
                    .RedirectWithSession(_sessionECampus)
                    .RedirectWithSession(_sessionLogin)
                    .RedirectWithSession(_sessionECampus, referer: LoginUrlBase, step: 4);
            }
            catch (FlurlHttpException e)
            {
                if (retry > 0)
                {
                    Thread.Sleep(500);
                    await LoginToECampus(retry - 1);
                }
                else
                {
                    throw new UserFriendlyException("Internal server error: " + e.Message);
                }
            }
        }

        public async Task<UserInfoDto> GetUserInfo()
        {
            await LoginToUis();
            await LoginToECampus();
            
            var info = await _sessionECampus
                .Request(UserInfoApiUrl)
                .WithFakeAgent()
                .WithHeader("referer", ECampusUrl)
                .WithHeader("accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9")
                .SetQueryParam("access_token", _sessionECampus.Cookies.First(c => c.Name == "access_token").Value)
                .WithAutoRedirect(false)
                .GetJsonAsync();

            return new UserInfoDto
            {
                StudentNumber = info.data.userInfo.id,
                Org = info.data.org.name,
                Mobile = info.data.userInfo.mobile,
                Email = info.data.userInfo.email,
                Gender = info.data.userInfo.gender == 1 ? UserGender.Male : UserGender.Female,
                Name = info.data.userInfo.name,
                StudentType= info.data.securityUserType.name
            };
        }
        

}
    
    public static class ExtendFlurl
    {
        private const string FakeUserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";

        public static IFlurlRequest WithFakeAgent(this IFlurlRequest req)
        {
            return req.WithHeader("User-Agent", FakeUserAgent);
        }
        
        public static async Task<IFlurlResponse> RedirectWithSession(this Task<IFlurlResponse> response, CookieSession session, int step = 1, string referer =  "")
        {
            while (true)
            {
                var rep = await response;

                if (step == 0) return rep;
                
                var redirectUrl = rep.Headers.GetAll("Location").First();

                if (referer != "")
                {
                    response = session
                        .Request(redirectUrl)
                        .WithFakeAgent()
                        .WithAutoRedirect(false)
                        .WithHeader("referer", referer)
                        .GetAsync();
                }
                else
                {
                    response = session
                        .Request(redirectUrl)
                        .WithFakeAgent()
                        .WithAutoRedirect(false)
                        .GetAsync();
                }
                step -= 1;
            }
        }
    }
}