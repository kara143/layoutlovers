using Abp.AutoMapper;
using layoutlovers.FilterTags.Dto;
using layoutlovers.ProductFilterTags.Dto.Base;
using layoutlovers.Products.Dto;

namespace layoutlovers.ProductFilterTags.Dto
{
    [AutoMap(typeof(ProductFilterTag))]
    public class ProductFilterTagDto: ProductFilterTagEntity
    {
        public ProductDto Product { get; set; }
        public FilterTagDto FilterTag { get; set; }
    }
}
