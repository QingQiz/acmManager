using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using acmManager.Configuration;
using acmManager.Web;

namespace acmManager.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class acmManagerDbContextFactory : IDesignTimeDbContextFactory<acmManagerDbContext>
    {
        public acmManagerDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<acmManagerDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            acmManagerDbContextConfigurer.Configure(builder, configuration.GetConnectionString(acmManagerConsts.ConnectionStringName));

            return new acmManagerDbContext(builder.Options);
        }
    }
}
