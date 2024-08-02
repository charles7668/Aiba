using Aiba.Enums;

namespace Aiba.Extensions
{
    public static class MediaTypeFlagExtension
    {
        public static bool HasMediaTypeFlag(this MediaTypeFlag flag, MediaTypeFlag value)
        {
            return (flag & value) == value;
        }
    }
}