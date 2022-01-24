using layoutlovers.FilterTags;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.ProductFilterTags
{
    public interface IProductFilterTagManager : IAppManagerBase<ProductFilterTag, Guid>
    {
        IEnumerable<FilterTag> GetFilterTagByProductId(Guid productId);
        Task CleanFilterTagByProductId(Guid productId);
    }
}