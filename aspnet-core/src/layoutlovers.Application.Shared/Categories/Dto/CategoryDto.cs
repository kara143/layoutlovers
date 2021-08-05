using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.Categories.Dto
{
    public class CategoryDto : EntityDto<Guid>, ICategory
    {
        public string Name { get; set; }
    }
}