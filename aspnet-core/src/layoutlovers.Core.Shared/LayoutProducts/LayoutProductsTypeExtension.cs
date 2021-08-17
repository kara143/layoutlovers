namespace layoutlovers.LayoutProducts
{
    public static class LayoutProductsTypeExtension
    {
        public static bool IsNotDefault(this LayoutProductType productType)
        {
            return productType != LayoutProductType.Default;
        }

        public static bool IsFree(this LayoutProductType productType)
        {
            return productType == LayoutProductType.Free;
        }

        public static bool IsBasic(this LayoutProductType productType)
        {
            return productType == LayoutProductType.Basic;
        }

        public static bool IsPremium(this LayoutProductType productType)
        {
            return productType == LayoutProductType.Premium;
        }
    }
}
