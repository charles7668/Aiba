using Microsoft.AspNetCore.Identity;

namespace Aiba.Entities
{
    public class UserSettingEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public string CoverImage { get; set; } = string.Empty;
    }
}