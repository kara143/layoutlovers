using System.Collections.Generic;

namespace layoutlovers.ShoppingCarts.Dto
{
    public class GetShoppingCartDto
    {
        public IEnumerable<ShoppingCartDto> ShoppingCartDtos { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
