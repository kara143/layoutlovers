using Abp.Modules;
using Abp.Reflection.Extensions;

namespace layoutlovers
{
    [DependsOn(typeof(layoutloversCoreSharedModule))]
    public class layoutloversApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversApplicationSharedModule).GetAssembly());
        }
    }
}