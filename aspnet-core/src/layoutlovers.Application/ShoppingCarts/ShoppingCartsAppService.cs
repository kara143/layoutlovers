using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using layoutlovers.ShoppingCarts.Dto;
using System;

namespace layoutlovers.ShoppingCarts
{
    public class ShoppingCartsAppService: CrudAppServiceBase<ShoppingCart
            , ShoppingCartDto
            , Guid
            , PagedAndSortedResultRequestDto
            , CreateShoppingCartDto
            , UpdateShoppingCartDto>, IShoppingCartsAppService
    {
        public ShoppingCartsAppService(IRepository<ShoppingCart, Guid> repository) : base(repository) { }
    }
}
