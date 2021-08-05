using Abp.Application.Services.Dto;
using layoutlovers.ShoppingCarts.Dto;
using System;

namespace layoutlovers.ShoppingCarts
{
    public interface IShoppingCartsAppService : ICrudAppServiceBase<ShoppingCartDto
        , Guid
        , PagedAndSortedResultRequestDto
        , CreateShoppingCartDto
        , UpdateShoppingCartDto>
    { }
}
