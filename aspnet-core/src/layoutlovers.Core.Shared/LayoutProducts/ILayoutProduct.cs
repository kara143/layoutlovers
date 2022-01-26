using System;

namespace layoutlovers.LayoutProducts
{
    public interface ILayoutProduct: INameBase
    {
        bool IsFeatured { get; set; }
        int FeaturedOrder { get; set; }
        decimal Amount { get; set; }
        string Description { get; set; }
        Guid CategoryId { get; set; }
        LayoutProductType LayoutProductType { get; set; }
        AlternativeGridType AlternativeGridType { get; set; }
    }
}
