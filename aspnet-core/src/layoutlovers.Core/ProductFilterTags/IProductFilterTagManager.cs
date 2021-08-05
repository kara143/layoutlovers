using layoutlovers.FilterTags;
using System;
using System.Collections.Generic;

namespace layoutlovers.ProductFilterTags
{
    public interface IProductFilterTagManager : IAppManagerBase<ProductFilterTag, Guid>
    {
        IEnumerable<FilterTag> GetFilterTagByProductId(Guid productId);
    }
}