using Abp.Dependency;

namespace layoutlovers.Jobs.Configuration
{
    public interface IJobsConfiguration : ITransientDependency
    {
        bool IsNewProductsNotification { get; }
    }
}
