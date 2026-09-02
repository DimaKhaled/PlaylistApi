namespace PlaylistApi.Domain.Entities
{
    public class Song
    {
        public int Id { get; set; }

        public long ExternalId { get; set; }

        public string Title { get; set; } 

        public string ArtistName { get; set; } 

        public string? AlbumName { get; set; }

        public int? DurationSeconds { get; set; }

        public string? ArtworkUrl { get; set; }

        public string? ExternalUrl { get; set; }

        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
