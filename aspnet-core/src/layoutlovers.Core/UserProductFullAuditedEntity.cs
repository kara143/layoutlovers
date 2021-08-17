using Abp.Domain.Entities.Auditing;
using layoutlovers.Authorization.Users;
using layoutlovers.LayoutProducts;
using System;

namespace layoutlovers
{
    public class UserProductFullAuditedEntity<TPrimaryKey> : FullAuditedEntity<TPrimaryKey>
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public Guid LayoutProductId { get; set; }
        public virtual LayoutProduct LayoutProduct { get; set; }
    }
}
