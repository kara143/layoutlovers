using Abp.Modules;
using Abp.Reflection.Extensions;

namespace layoutlovers
{
    public class layoutloversCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversCoreSharedModule).GetAssembly());
        }
    }
}