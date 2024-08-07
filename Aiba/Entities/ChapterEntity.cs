using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aiba.Entities
{
    [Index(nameof(Title))]
    [Index(nameof(MediaInfoId))]
    public class ChapterEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Url { get; set; } = string.Empty;

        public int MediaInfoId { get; set; }
        public MediaInfoEntity MediaInfo { get; set; } = new();
    }
}