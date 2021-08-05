using Abp.AutoMapper;
using layoutlovers.FilterTags.Dto.Base;

namespace layoutlovers.FilterTags.Dto
{
    [AutoMap(typeof(FilterTag))]
    public class FilterTagDto: FilterTagEntity
    {
    }
}
