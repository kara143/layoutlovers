using System.ComponentModel;

namespace layoutlovers.MultiTenancy.Payments
{
    public enum RequestPayerStatus
    {
        [Description("Successful")]
        Successful = 1,
        [Description("Pending")]
        Pending,
        [Description("Cancelled")]
        Cancelled,
        [Description("Failed")]
        Failed,
    }
}
