using Abp.Modules;
using Abp.Reflection.Extensions;

namespace layoutlovers
{
    [DependsOn(typeof(layoutloversXamarinSharedModule))]
    public class layoutloversXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversXamarinIosModule).GetAssembly());
        }
    }
}