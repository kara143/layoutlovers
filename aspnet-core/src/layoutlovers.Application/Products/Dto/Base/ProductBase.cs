using System;

namespace layoutlovers.Products.Dto.Base
{
    public class ProductBase: IProduct
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
