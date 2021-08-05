using Abp.Application.Services.Dto;
using layoutlovers.Favorites.Dto;
using System;

namespace layoutlovers.Favorites
{
    public interface IFavoritesAppService : ICrudAppServiceBase<FavoriteDto
        , Guid
        , PagedAndSortedResultRequestDto
        , CreateFavoriteDto
        , UpdateFavoriteDto>
    { }
}
