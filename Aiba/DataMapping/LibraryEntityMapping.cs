using Aiba.Entities;
using Aiba.Model;

namespace Aiba.DataMapping
{
    public static class LibraryEntityMapping
    {
        public static void Map(LibraryEntity input, LibraryInfo output)
        {
            output.Name = input.Name;
            output.Path = input.Path;
            output.Type = input.Type;
        }

        public static void MapFrom(LibraryEntity output, LibraryInfo input)
        {
            output.Name = input.Name;
            output.Path = input.Path;
            output.Type = input.Type;
        }
    }
}