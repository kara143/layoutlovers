using Abp.AutoMapper;
using layoutlovers.Categories.Dto;
using layoutlovers.Files.Dto;
using layoutlovers.FilterTags.Dto;
using layoutlovers.LayoutProducts.Dto.Base;
using System.Collections.Generic;

namespace layoutlovers.LayoutProducts.Dto
{
    [AutoMap(typeof(LayoutProduct))]
    public class LayoutProductDto: LayoutProductEnity
    {
        public virtual CategoryDto Category { get; set; }
        public virtual ICollection<S3FileDtoBase> AmazonS3Files { set; get; }
        public IEnumerable<FilterTagDto> FilterTagDtos { get; set; }
    }
}
