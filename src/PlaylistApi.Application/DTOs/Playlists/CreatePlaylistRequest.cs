using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.Application.DTOs.Playlists
{
    public class CreatePlaylistRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
