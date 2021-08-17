using Abp.AutoMapper;
using layoutlovers.Purchases.Dto.Base;

namespace layoutlovers.Purchases.Dto
{
    [AutoMap(typeof(Purchase))]
    public class PurchaseDto: PurchaseEntity
    {
    }
}
