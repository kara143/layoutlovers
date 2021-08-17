using System;

namespace layoutlovers.LayoutProducts.Dto.Base
{
    public class LayoutProductBase: ILayoutProduct
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public LayoutProductType LayoutProductType { get; set; }
    }
}
