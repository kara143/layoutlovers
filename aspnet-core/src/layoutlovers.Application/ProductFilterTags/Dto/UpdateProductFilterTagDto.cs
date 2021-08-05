using Abp.AutoMapper;
using layoutlovers.ProductFilterTags.Dto.Base;

namespace layoutlovers.ProductFilterTags.Dto
{
    [AutoMapTo(typeof(ProductFilterTag))]
    public class UpdateProductFilterTagDto: ProductFilterTagEntity
    {
    }
}
