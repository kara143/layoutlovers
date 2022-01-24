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

            var dtos = MapToDto(entities);

            return dtos;
        }

        public IEnumerable<FilterTagDto> GetAllByCategoryId(Guid id)
        {
            var entities = _filterTagManager.GetFilterTagByCategoryId(id);

            var dtos = MapToDto(entities);

            return dtos;
        }

        public IEnumerable<FilterTagDto> GetAllForFeatured()
        {
            var entities = _filterTagManager.GetFilterTagForFeatured();

            var dtos = MapToDto(entities);

            return dtos;
        }

        private IEnumerable<FilterTagDto> MapToDto(IEnumerable<FilterTag> entities)
        {
            return ObjectMapper.Map<IEnumerable<FilterTagDto>>(entities);
        }
    }
}
