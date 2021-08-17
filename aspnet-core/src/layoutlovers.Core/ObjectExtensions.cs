namespace layoutlovers.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull<TObj>(this TObj obj) where TObj: class
        {
            return obj == null;
        }

        public static bool IsNotNull<TObj>(this TObj obj) where TObj : class
        {
            return !IsNull(obj);
        }
    }
}
