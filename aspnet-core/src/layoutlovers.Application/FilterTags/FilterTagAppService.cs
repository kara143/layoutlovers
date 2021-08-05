using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using layoutlovers.FilterTags.Dto;
using layoutlovers.ProductFilterTags;
using System;
using System.Collections.Generic;

namespace layoutlovers.FilterTags
{
    public class FilterTagAppService: CrudAppServiceBase<FilterTag
            , FilterTagDto
            , Guid
            , PagedAndSortedResultRequestDto
            , CreateFilterTagDto
            , UpdateFilterTagDto>, IFilterTagAppService
    {
        private readonly IProductFilterTagManager _filterTagManager;
        public FilterTagAppService(IRepository<FilterTag, Guid> repository
            , IProductFilterTagManager filterTagManager) : base(repository)
        {
            _filterTagManager = filterTagManager;
        }

        public IEnumerable<FilterTagDto> GetAllByProductId(Guid id)
        {
            var entities = _filterTagManager.GetFilterTagByProductId(id);

            var dtos = ObjectMapper.Map<IEnumerable<FilterTagDto>>(entities);

            return dtos;
        }
    }
}
