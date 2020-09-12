using System.Collections.Generic;
using System.IO;
using Abp.Configuration;

namespace acmManager.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var crawlerPath = Directory.GetParent(Directory.GetCurrentDirectory()) + @"\crawler.py";
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.CrawlerPath, crawlerPath, scopes: SettingScopes.All, isVisibleToClients: true)
            };
        }
    }
}
