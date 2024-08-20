namespace Aiba.Helpers
{
    public static class PathHelper
    {
        public static string TrimFileProtocol(this string path)
        {
            return path.StartsWith("file://") ? path[7..] : path;
        }

        public static string ToFileProtocol(this string path)
        {
            return "file://" + path.TrimFileProtocol();
        }
    }
}