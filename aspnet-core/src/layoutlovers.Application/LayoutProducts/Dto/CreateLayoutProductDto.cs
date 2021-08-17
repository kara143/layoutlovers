using Abp.AutoMapper;
using layoutlovers.FilterTags.Dto;
using layoutlovers.LayoutProducts.Dto.Base;
using System.Collections.Generic;

namespace layoutlovers.LayoutProducts.Dto
{
    [AutoMapTo(typeof(LayoutProduct))]
    public class CreateLayoutProductDto : LayoutProductBase
    {
        public IEnumerable<FilterTagDto> FilterTagDtos {get; set;}
    }
}
