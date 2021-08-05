using Abp.AutoMapper;
using layoutlovers.FilterTags.Dto;
using layoutlovers.Products.Dto.Base;
using System.Collections.Generic;

namespace layoutlovers.Products.Dto
{
    [AutoMapTo(typeof(Product))]
    public class CreateProductDto : ProductBase
    {
        public IEnumerable<FilterTagDto> FilterTagDtos {get; set;}
    }
}
