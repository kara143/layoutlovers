using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.FilterTags.Dto.Base
{
    public class FilterTagEntity : FilterTagBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
