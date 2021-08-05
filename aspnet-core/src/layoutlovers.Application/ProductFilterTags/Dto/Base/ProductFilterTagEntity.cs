using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.ProductFilterTags.Dto.Base
{
    public class ProductFilterTagEntity : ProductFilterTagBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
