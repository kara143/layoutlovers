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
        public bool IsPurchased { get; set; }
        public CategoryDto Category { get; set; }
        public IEnumerable<S3ImageDto> AmazonS3Files { set; get; }
        public IList<FilterTagDto> FilterTagDtos { get; set; }
    }
}
