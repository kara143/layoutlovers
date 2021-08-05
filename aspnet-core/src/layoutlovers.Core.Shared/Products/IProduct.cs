using System;

namespace layoutlovers.Products
{
    public interface IProduct: INameBase
    {
        decimal Amount { get; set; }
        string Description { get; set; }
        Guid CategoryId { get; set; }
        ProductType ProductType { get; set; }
    }
}
