namespace PlaylistApi.Application.DTOs.Playlists
{
    public class PlaylistSongResponse
    {
        public int SongId { get; set; }

        public long ExternalId { get; set; }

        public string Title { get; set; }

        public string ArtistName { get; set; }

        public string? AlbumName { get; set; }

        public int? DurationSeconds { get; set; }

        public string? ArtworkUrl { get; set; }

        public string? ExternalUrl { get; set; }
    }
}
