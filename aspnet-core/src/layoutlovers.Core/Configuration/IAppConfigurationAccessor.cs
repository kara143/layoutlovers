using Microsoft.Extensions.Configuration;

namespace layoutlovers.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
