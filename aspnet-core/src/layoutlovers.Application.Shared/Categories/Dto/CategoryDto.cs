using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.Categories.Dto
{
    public class CategoryDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}