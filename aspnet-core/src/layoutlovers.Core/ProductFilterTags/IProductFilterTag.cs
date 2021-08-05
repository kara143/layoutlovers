using System;

namespace layoutlovers.ProductFilterTags
{
    public interface IProductFilterTag
    {
        Guid ProductId { get; set; }
        Guid FilterTagId { get; set; }
    }
}