using System.Threading.Tasks;
using acmManager.Configuration.Dto;

namespace acmManager.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
