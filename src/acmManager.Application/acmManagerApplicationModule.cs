using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using acmManager.Authorization;

namespace acmManager
{
    [DependsOn(
        typeof(acmManagerCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class acmManagerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<acmManagerAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(acmManagerApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
