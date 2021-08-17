using System;

namespace layoutlovers.ProductFilterTags
{
    public interface IProductFilterTag
    {
        Guid LayoutProductId { get; set; }
        Guid FilterTagId { get; set; }
    }
}