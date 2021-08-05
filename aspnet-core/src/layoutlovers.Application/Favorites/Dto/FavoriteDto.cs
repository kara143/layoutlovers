using Abp.AutoMapper;
using layoutlovers.Favorites.Dto.Base;

namespace layoutlovers.Favorites.Dto
{
    [AutoMap(typeof(Favorite))]
    public class FavoriteDto: FavoriteEntity
    {
    }
}
