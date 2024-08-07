using Aiba.Entities;

namespace Aiba.DataMapping
{
    public static class GenreEntityMapping
    {
        public static void Map(GenreEntity input, out string output)
        {
            output = input.Name;
        }

        public static void MapFrom(GenreEntity output, string input)
        {
            output.Name = input;
        }
    }
}