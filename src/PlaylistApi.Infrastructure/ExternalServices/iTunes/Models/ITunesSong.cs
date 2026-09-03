namespace PlaylistApi.Infrastructure.ExternalServices.iTunes.Models
{
    public class ITunesSong
    {
        public long TrackId { get; set; }

        public string? TrackName { get; set; }

        public string? ArtistName { get; set; }

        public string? CollectionName { get; set; }

        public long? TrackTimeMillis { get; set; }

        public string? ArtworkUrl100 { get; set; }

        public string? TrackViewUrl { get; set; }

        public string? PreviewUrl { get; set; }
    }
}
