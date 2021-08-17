using Abp.Domain.Entities.Auditing;
using layoutlovers.Editions;
using layoutlovers.LayoutProducts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace layoutlovers.DownloadRestrictions
{
    [Table("AppDownloadRestrictions")]
    public class DownloadRestriction : FullAuditedEntity<Guid>, IDownloadRestriction
    {
        public int DownloadPerDay { get; set; }
        public LayoutProductType LayoutProductType { get; set; }
        public virtual SubscribableEdition SubscribableEdition { get; set; }
        public int SubscribableEditionId { get; set; }
    }
}
