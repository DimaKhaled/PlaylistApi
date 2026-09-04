using PlaylistApi.Application.DTOs.Songs;
using PlaylistApi.Application.Interfaces.ExternalServices;
using PlaylistApi.Infrastructure.ExternalServices.iTunes.Models;
using System.Net.Http.Json;

namespace PlaylistApi.Infrastructure.ExternalServices.iTunes
{
    public class ITunesMusicService : IMusicService
    {
        private readonly HttpClient _httpClient;

        public ITunesMusicService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<SongSearchResponse>> SearchAsync(string query)
        {
            var encodedQuery = Uri.EscapeDataString(query);

            var response = await _httpClient.GetAsync($"search?term={encodedQuery}&entity=song&limit=25");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ITunesResponse>();

            if (result is null)
            {
                return [];
            }

            return result.Results.Where(IsValidSong).Select(MapToSearchResponse).ToList();
        }


        public async Task<SongSearchResponse?> GetByExternalIdAsync(long externalId)
        {

            var response = await _httpClient.GetAsync($"lookup?id={externalId}&entity=song");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ITunesResponse>();

            if (result is null)
            {
                return null;
            }

            var song = result.Results.FirstOrDefault(IsValidSong);

            if (song == null) 
            {
                return null;
            }

            return MapToSearchResponse(song);
        }



        private static bool IsValidSong(ITunesSong song)
        {
            return song.TrackId > 0 && !string.IsNullOrWhiteSpace(song.TrackName) &&
                   !string.IsNullOrWhiteSpace(song.ArtistName);
        }


        private static SongSearchResponse MapToSearchResponse(ITunesSong song)
        {
            return new SongSearchResponse
            {
                ExternalId = song.TrackId,
                Title = song.TrackName!,
                ArtistName = song.ArtistName!,
                AlbumName = song.CollectionName,

                DurationSeconds = song.TrackTimeMillis.HasValue
                    ? (int)Math.Round(song.TrackTimeMillis.Value / 1000.0) : null,

                ArtworkUrl = song.ArtworkUrl100,
                ExternalUrl = song.TrackViewUrl,
                PreviewUrl = song.PreviewUrl
            };
        }
    }
}
