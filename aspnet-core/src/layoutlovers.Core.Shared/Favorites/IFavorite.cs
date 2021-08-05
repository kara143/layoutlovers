using System;

namespace layoutlovers.UserProducts
{
    public interface IFavorite
    {
        Guid ProductId { get; set; }
        long UserId { get; set; }
    }
}
