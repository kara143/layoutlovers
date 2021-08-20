using Abp.AutoMapper;
using layoutlovers.PurchaseItems.Dto.Base;

namespace layoutlovers.PurchaseItems.Dto
{
    [AutoMap(typeof(PurchaseItem))]
    public class PurchaseItemDto: PurchaseItemEntity
    {
    }
}
