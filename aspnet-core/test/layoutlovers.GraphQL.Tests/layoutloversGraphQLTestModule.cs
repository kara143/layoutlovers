using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using layoutlovers.Configure;
using layoutlovers.Startup;
using layoutlovers.Test.Base;

namespace layoutlovers.GraphQL.Tests
{
    [DependsOn(
        typeof(layoutloversGraphQLModule),
        typeof(layoutloversTestBaseModule))]
    public class layoutloversGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(layoutloversGraphQLTestModule).GetAssembly());
        }
    }
}