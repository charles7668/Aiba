using Aiba.Entities;
using Aiba.Model;

namespace Aiba.DataMapping
{
    public static class ChapterEntityMapping
    {
        public static void Map(ChapterEntity input, ChapterInfo output)
        {
            output.ChapterName = input.Title;
            output.ChapterUrl = input.Url;
        }

        public static void MapFrom(ChapterEntity output, ChapterInfo input)
        {
            output.Title = input.ChapterName;
            output.Url = input.ChapterUrl;
        }
    }
}