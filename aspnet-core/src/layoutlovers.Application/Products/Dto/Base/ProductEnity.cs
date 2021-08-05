using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.Products.Dto.Base
{
    public class ProductEnity : ProductBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
