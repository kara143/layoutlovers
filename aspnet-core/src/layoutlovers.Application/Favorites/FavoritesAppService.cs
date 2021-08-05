using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using layoutlovers.Favorites.Dto;
using System;

namespace layoutlovers.Favorites
{
    public class FavoritesAppService :
        CrudAppServiceBase<Favorite
            , FavoriteDto
            , Guid
            , PagedAndSortedResultRequestDto
            , CreateFavoriteDto
            , UpdateFavoriteDto>, IFavoritesAppService
    {
        public FavoritesAppService(IRepository<Favorite, Guid> repository) : base(repository)
        {
        }
    }
}
