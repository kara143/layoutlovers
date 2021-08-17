using layoutlovers.LayoutProducts;

namespace layoutlovers.DownloadRestrictions
{
    public interface IDownloadRestriction
    {
        int DownloadPerDay { get; set; }
        LayoutProductType LayoutProductType { get; set; }
        int SubscribableEditionId { get; set; }
    }
}
