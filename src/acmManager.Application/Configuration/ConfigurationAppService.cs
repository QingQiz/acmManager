using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using acmManager.Configuration.Dto;

namespace acmManager.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : acmManagerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
