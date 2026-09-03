using Microsoft.AspNetCore.Identity;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<Playlist> playlists { get; set; } = new List<Playlist>();
    }
}
