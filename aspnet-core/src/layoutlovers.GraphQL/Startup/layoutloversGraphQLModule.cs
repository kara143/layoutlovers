using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace layoutlovers.Startup
{
    [DependsOn(typeof(layoutloversCoreModule))]
    public class layoutloversGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}