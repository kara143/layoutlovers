using Abp.AutoMapper;
using layoutlovers.FilterTags.Dto;
using layoutlovers.LayoutProducts.Dto;
using layoutlovers.ProductFilterTags.Dto.Base;

namespace layoutlovers.ProductFilterTags.Dto
{
    [AutoMap(typeof(ProductFilterTag))]
    public class ProductFilterTagDto: ProductFilterTagEntity
    {
        public LayoutProductDto LayoutProduct { get; set; }
        public FilterTagDto FilterTag { get; set; }
    }
}
