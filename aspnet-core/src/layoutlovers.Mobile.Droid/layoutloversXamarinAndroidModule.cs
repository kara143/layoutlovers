using Abp.Modules;
using Abp.Reflection.Extensions;

namespace layoutlovers
{
    [DependsOn(typeof(layoutloversXamarinSharedModule))]
    public class layoutloversXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversXamarinAndroidModule).GetAssembly());
        }
    }
}