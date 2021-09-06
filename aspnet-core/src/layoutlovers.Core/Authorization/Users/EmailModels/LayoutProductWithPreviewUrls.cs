using layoutlovers.LayoutProducts;
using System.Collections.Generic;

namespace layoutlovers.Authorization.Users.EmailModels
{
    public class LayoutProductWithPreviewUrls
    {
        public LayoutProduct LayoutProduct { get; set; }
        public IEnumerable<string> PreviewUrls { get; set; }
    }
}
