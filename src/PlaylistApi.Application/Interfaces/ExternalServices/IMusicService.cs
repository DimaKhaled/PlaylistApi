using PlaylistApi.Application.DTOs.Songs;

namespace PlaylistApi.Application.Interfaces.ExternalServices
{
    public interface IMusicService
    {
        Task<List<SongSearchResponse>> SearchAsync(string query);

        Task<SongSearchResponse?> GetByExternalIdAsync(long externalId);
    }
}
