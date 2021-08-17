using System;

namespace layoutlovers.LayoutProducts
{
    public interface ILayoutProduct: INameBase
    {
        decimal Amount { get; set; }
        string Description { get; set; }
        Guid CategoryId { get; set; }
        LayoutProductType LayoutProductType { get; set; }
    }
}
