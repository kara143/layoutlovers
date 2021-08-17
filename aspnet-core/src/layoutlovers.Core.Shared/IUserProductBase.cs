using System;

namespace layoutlovers
{
    public interface IUserProductBase
    {
        long UserId { get; set; }
        Guid LayoutProductId { get; set; }
    }
}
