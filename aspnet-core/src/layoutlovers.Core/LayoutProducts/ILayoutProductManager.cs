using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace layoutlovers.LayoutProducts
{
    public interface ILayoutProductManager : IAppManagerBase<LayoutProduct, Guid>
    {
        Task<LayoutProduct> GetById(Guid id);
        LayoutProduct GetByIdAndIncluding(Guid id);
    }
}
