using Abp.Application.Services.Dto;
using System;

namespace layoutlovers.ShoppingCarts.Dto.Base
{
    public class ShoppingCartEntity : ShoppingCartBase, IEntityDto<Guid>
    {
        public Guid Id { get; set; }
    }
}
