using Abp.AutoMapper;
using layoutlovers.ShoppingCarts.Dto.Base;

namespace layoutlovers.ShoppingCarts.Dto
{
    [AutoMapTo(typeof(ShoppingCart))]
    public class UpdateShoppingCartDto: ShoppingCartEntity
    {
    }
}
