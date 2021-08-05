namespace layoutlovers.Products
{
    public static class ProductTypeExtension
    {
        public static bool IsNotDefault(this ProductType productType)
        {
            return productType != ProductType.Default;
        }
    }
}
