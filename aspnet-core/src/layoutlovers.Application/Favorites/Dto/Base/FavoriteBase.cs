using layoutlovers.UserProducts;
using System;

namespace layoutlovers.Favorites.Dto.Base
{
    public class FavoriteBase : IFavorite
    {
        public Guid ProductId { get; set; }
        public long UserId { get; set; }
    }
}
