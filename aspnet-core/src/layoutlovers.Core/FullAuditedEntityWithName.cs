using Abp.Domain.Entities.Auditing;

namespace layoutlovers
{
    public class FullAuditedEntityWithName<TPrimaryKey> : FullAuditedEntity<TPrimaryKey>, INameBase
    {
        public string Name { get; set; }
    }
}
