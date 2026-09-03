namespace PlaylistApi.Infrastructure.ExternalServices.iTunes.Models
{
    public class ITunesResponse
    {
        public int ResultCount { get; set; }

        public List<ITunesSong> Results { get; set; } = [];
    }
}
