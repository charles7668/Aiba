using Aiba.Enums;
using System.Text;

namespace Aiba.Model.Extensions
{
    public static class MediaTypeFlagExtension
    {
        public static string GetMediaTypeString(this MediaTypeFlag flag)
        {
            var sb = new StringBuilder(100);
            if (flag.HasMediaTypeFlag(MediaTypeFlag.MANGA))
                sb.Append("manga|");
            if (flag.HasMediaTypeFlag(MediaTypeFlag.VIDEO))
                sb.Append("video|");
            // remove last |
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static bool HasMediaTypeFlag(this MediaTypeFlag flag, MediaTypeFlag value)
        {
            return (flag & value) == value;
        }
    }
}