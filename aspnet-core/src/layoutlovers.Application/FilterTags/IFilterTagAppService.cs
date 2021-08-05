using Abp.Application.Services.Dto;
using layoutlovers.FilterTags.Dto;
using System;
using System.Collections.Generic;

namespace layoutlovers.FilterTags
{
    public interface IFilterTagAppService : ICrudAppServiceBase<FilterTagDto
            , Guid
            , PagedAndSortedResultRequestDto
            , CreateFilterTagDto
            , UpdateFilterTagDto>
    {
        IEnumerable<FilterTagDto> GetAllByProductId(Guid id);
    }
}