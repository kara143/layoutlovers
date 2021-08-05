using Abp.Domain.Entities.Auditing;
using layoutlovers.Authorization.Users;
using layoutlovers.Products;
using layoutlovers.UserProducts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.Favorites
{
    [Table("AppFavorites")]
    public class Favorite: FullAuditedEntity<Guid>, IFavorite
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
