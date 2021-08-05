using Abp.Domain.Repositories;
using System;

namespace layoutlovers.FilterTags
{
    public class FilterTagManager: AppManagerBase<FilterTag, Guid>, IFilterTagManager
    {
        public FilterTagManager(IRepository<FilterTag, Guid> repository): base(repository)
        {
        }
    }
}
