using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.Purchases.Dto.Base
{
    public class PurchaseEntity : PurchaseBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
