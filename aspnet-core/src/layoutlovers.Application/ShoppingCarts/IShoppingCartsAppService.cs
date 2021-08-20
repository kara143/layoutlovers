using Abp.Application.Services.Dto;
using layoutlovers.MultiTenancy.Payments.Stripe.Dto;
using layoutlovers.Purchases.Dto;
using layoutlovers.ShoppingCarts.Dto;
using System;
using System.Threading.Tasks;

namespace layoutlovers.ShoppingCarts
{
    public interface IShoppingCartsAppService : ICrudAppServiceBase<ShoppingCartDto
        , Guid
        , PagedAndSortedResultRequestDto
        , CreateShoppingCartDto
        , UpdateShoppingCartDto>
    {
        Task<PurchaseDto> BuyShoppingCart(BuyProduct buyProduct);
        Task<GetShoppingCartDto> GetCurrentUserShoppingCart();
    }
}
