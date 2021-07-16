using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using layoutlovers.Authorization;

namespace layoutlovers
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(layoutloversApplicationSharedModule),
        typeof(layoutloversCoreModule)
        )]
    public class layoutloversApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversApplicationModule).GetAssembly());
        }
    }
}