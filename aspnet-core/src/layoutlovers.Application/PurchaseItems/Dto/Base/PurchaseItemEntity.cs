using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.PurchaseItems.Dto.Base
{
    public class PurchaseItemEntity : PurchaseItemBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
