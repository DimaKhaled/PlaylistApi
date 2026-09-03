namespace PlaylistApi.Application.DTOs.Songs
{
    public class SongSearchResponse
    {
        public long ExternalId { get; set; }

        public string Title { get; set; } 

        public string ArtistName { get; set; } 

        public string? AlbumName { get; set; }

        public int? DurationSeconds { get; set; }

        public string? ArtworkUrl { get; set; }

        public string? ExternalUrl { get; set; }

        public string? PreviewUrl { get; set; }
    }
}
