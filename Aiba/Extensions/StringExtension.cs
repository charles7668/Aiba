namespace Aiba.Extensions
{
    public static class StringExtension
    {
        public static bool IsHttpLink(this string url)
        {
            return url.StartsWith("http://") || url.StartsWith("https://");
        }
    }
}