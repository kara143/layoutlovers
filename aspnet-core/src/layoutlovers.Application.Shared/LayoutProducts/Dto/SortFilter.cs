using System.ComponentModel;

namespace layoutlovers.LayoutProducts.Dto
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
        Premium,
        [Description("Popular")]
        Popular
    }
}
