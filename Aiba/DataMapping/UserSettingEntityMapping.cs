using Aiba.Entities;
using Aiba.Model;

namespace Aiba.DataMapping
{
    public static class UserSettingEntityMapping
    {
        public static void Map(UserSettingEntity input, UserSetting output)
        {
            output.CoverImage = input.CoverImage;
        }

        public static void MapFrom(UserSettingEntity output, UserSetting input)
        {
            output.CoverImage = input.CoverImage;
        }
    }
}