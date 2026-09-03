using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.Application.DTOs.Songs
{
    public class AddSongRequest
    {
        [Required]
        public long ExternalId { get; set; }
    }
}
