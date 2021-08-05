using Abp.AutoMapper;
using layoutlovers.ShoppingCarts.Dto.Base;

namespace layoutlovers.ShoppingCarts.Dto
{
    [AutoMap(typeof(ShoppingCart))]
    public class ShoppingCartDto: ShoppingCartEntity
    {
    }
}
