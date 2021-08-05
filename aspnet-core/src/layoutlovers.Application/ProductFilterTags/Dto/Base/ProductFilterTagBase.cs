using System;

namespace layoutlovers.ProductFilterTags.Dto.Base
{
    public class ProductFilterTagBase : IProductFilterTag
    {
        public Guid ProductId { get; set; }
        public Guid FilterTagId { get; set; }
    }
}
