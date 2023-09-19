using Microsoft.AspNetCore.Identity;
using MyBlog.Domain.Entities;

namespace MyBlog.Persistance.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string UserProfileId { get; set; } = null!;
        public UserProfile UserProfile { get; set; } = null!;

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
