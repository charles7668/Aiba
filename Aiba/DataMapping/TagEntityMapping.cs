using Aiba.Entities;

namespace Aiba.DataMapping
{
    public static class TagEntityMapping
    {
        public static void Map(TagEntity input, out string output)
        {
            output = input.Name;
        }

        public static void MapFrom(TagEntity output, string input)
        {
            output.Name = input;
        }
    }
}