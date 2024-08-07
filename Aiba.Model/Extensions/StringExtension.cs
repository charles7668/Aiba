using Aiba.Enums;
using Aiba.Model.Constants;

namespace Aiba.Model.Extensions
{
    public static class StringExtension
    {
        public static MediaTypeFlag GetFlag(this string typeString)
        {
            string[] splitTypeString = typeString.Split('|').Select(x => x.Trim()).ToArray();
            MediaTypeFlag flag = 0;
            foreach (string type in splitTypeString)
            {
                switch (type)
                {
                    case MediaInfoType.MANGA or MediaInfoType.ALL:
                        flag |= MediaTypeFlag.MANGA;
                        break;
                    case MediaInfoType.VIDEO or MediaInfoType.ALL:
                        flag |= MediaTypeFlag.VIDEO;
                        break;
                }
            }

            return flag;
        }
    }
}