using Aiba.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aiba.Entities
{
    [Index(nameof(Name))]
    [Index(nameof(Author))]
    [Index(nameof(Type))]
    [Index(nameof(LibraryId))]
    public class MediaInfoEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string[] Author { get; set; } = [];

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(50)]
        public MediaTypeFlag Type { get; set; } = MediaTypeFlag.MANGA;

        public GenreEntity[] Genres { get; set; } = [];

        public TagEntity[] Tags { get; set; } = [];

        public ChapterEntity[] Chapters { get; set; } = [];

        [MaxLength(255)]
        public string ProviderName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ProviderUrl { get; set; } = string.Empty;

        public int LibraryId { get; set; }
        public LibraryEntity Library { get; set; } = null!;
    }
}