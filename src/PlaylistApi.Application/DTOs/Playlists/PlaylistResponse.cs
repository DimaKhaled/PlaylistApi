namespace PlaylistApi.Application.DTOs.Playlists
{
    public class PlaylistResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<PlaylistSongResponse> Songs { get; set; } = [];
    
    }
}
