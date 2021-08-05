using Abp.Domain.Repositories;
using System;

namespace layoutlovers.Favorites
{
    public class FavoriteManager : AppManagerBase<Favorite, Guid>, IFavoriteManager
    {
        public FavoriteManager(IRepository<Favorite, Guid> repository) : base(repository)
        {
        }
    }
}
