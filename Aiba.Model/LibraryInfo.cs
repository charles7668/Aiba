using Aiba.Enums;

namespace Aiba.Model
{
    public class LibraryInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public MediaTypeFlag Type { get; set; }
    }
}