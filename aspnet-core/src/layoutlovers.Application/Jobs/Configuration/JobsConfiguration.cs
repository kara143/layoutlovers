using Abp.Extensions;
using layoutlovers.Configuration;
using Microsoft.Extensions.Configuration;

namespace layoutlovers.Jobs.Configuration
{
    public class JobsConfiguration : IJobsConfiguration
    {
        private readonly IConfigurationRoot _appConfiguration;
        public bool IsNewProductsNotification => _appConfiguration["email:IsNewProductsNotification"].To<bool>();

        public JobsConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}
