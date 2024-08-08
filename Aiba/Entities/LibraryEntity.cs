using Aiba.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aiba.Entities
{
    [Index(nameof(UserId))]
    [Index(nameof(Name))]
    [Index(nameof(Path))]
    [Index(nameof(UserId), nameof(Name), IsUnique = true)]
    [Index(nameof(UserId), nameof(Path), IsUnique = true)]
    [Index(nameof(UserId), nameof(Type))]
    public class LibraryEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Path { get; set; } = string.Empty;

        [Required]
        public MediaTypeFlag Type { get; set; } = MediaTypeFlag.MANGA;

        [MaxLength(255)]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ScannerName { get; set; } = string.Empty;

        public IdentityUser User { get; set; } = null!;
    }
}