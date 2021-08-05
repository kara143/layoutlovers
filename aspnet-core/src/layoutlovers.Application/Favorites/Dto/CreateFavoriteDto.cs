using Abp.AutoMapper;
using layoutlovers.Favorites.Dto.Base;

namespace layoutlovers.Favorites.Dto
{
    [AutoMapTo(typeof(Favorite))]
    public class CreateFavoriteDto: FavoriteBase
    {
    }
}
