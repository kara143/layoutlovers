using System.ComponentModel;

namespace layoutlovers.Products.Dto
{
    public enum SortFilter
    {
        [Description("Default")]
        Default,
        [Description("High")]
        High,
        [Description("Low")]
        Low,
        [Description("Free")]
        Free,
        [Description("Premium")]
        Premium
    }
}
