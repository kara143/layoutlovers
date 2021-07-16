using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace layoutlovers
{
    [DependsOn(typeof(layoutloversClientModule), typeof(AbpAutoMapperModule))]
    public class layoutloversXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversXamarinSharedModule).GetAssembly());
        }
    }
}