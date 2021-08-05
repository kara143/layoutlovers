using Abp.AutoMapper;
using layoutlovers.FilterTags.Dto.Base;

namespace layoutlovers.FilterTags.Dto
{
    [AutoMapTo(typeof(FilterTag))]
    public class UpdateFilterTagDto: FilterTagEntity
    {
    }
}
