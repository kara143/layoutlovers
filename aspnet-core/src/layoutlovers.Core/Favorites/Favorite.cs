using layoutlovers.UserProducts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Favorites
{
    [Table("AppFavorites")]
    public class Favorite: UserProductFullAuditedEntity<Guid>, IFavorite
    {
    }
}
