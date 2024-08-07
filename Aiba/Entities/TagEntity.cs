using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aiba.Entities
{
    [Index(nameof(Name))]
    public class TagEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}