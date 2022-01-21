using System;

namespace layoutlovers.LayoutProducts.Dto.Base
{
    public class LayoutProductBase: ILayoutProduct
    {
        public bool IsFeatured { get; set; }
        public int FeaturedOrder { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public LayoutProductType LayoutProductType { get; set; }
        public AlternativeGridType AlternativeGridType { get; set; }
    }
}
