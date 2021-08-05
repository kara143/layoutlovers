using Abp.AutoMapper;
using layoutlovers.Categories.Dto;
using layoutlovers.Files.Dto;
using layoutlovers.FilterTags.Dto;
using layoutlovers.Products.Dto.Base;
using System.Collections.Generic;

namespace layoutlovers.Products.Dto
{
    [AutoMap(typeof(Product))]
    public class ProductDto: ProductEnity
    {
        public virtual CategoryDto Category { get; set; }
        public virtual ICollection<S3FileDtoBase> AmazonS3Files { set; get; }
        public IEnumerable<FilterTagDto> FilterTagDtos { get; set; }
    }
}
