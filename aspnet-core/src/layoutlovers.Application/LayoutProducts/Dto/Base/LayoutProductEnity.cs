using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.LayoutProducts.Dto.Base
{
    public class LayoutProductEnity : LayoutProductBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
