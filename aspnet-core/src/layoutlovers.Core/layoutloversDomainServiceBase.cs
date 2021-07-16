using Abp.Domain.Services;

namespace layoutlovers
{
    public abstract class layoutloversDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected layoutloversDomainServiceBase()
        {
            LocalizationSourceName = layoutloversConsts.LocalizationSourceName;
        }
    }
}
