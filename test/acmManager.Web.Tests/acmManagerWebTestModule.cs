using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using acmManager.EntityFrameworkCore;
using acmManager.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace acmManager.Web.Tests
{
    [DependsOn(
        typeof(acmManagerWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class acmManagerWebTestModule : AbpModule
    {
        public acmManagerWebTestModule(acmManagerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(acmManagerWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(acmManagerWebMvcModule).Assembly);
        }
    }
}