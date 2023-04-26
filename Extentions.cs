namespace WebServerMPImages
{
    public static class Extentions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }        
    }
}
