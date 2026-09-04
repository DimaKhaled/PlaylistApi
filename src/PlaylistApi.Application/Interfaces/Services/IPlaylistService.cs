using PlaylistApi.Application.DTOs.Playlists;

namespace PlaylistApi.Application.Interfaces.Services
{
    public interface IPlaylistService
    {
        Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request);

        Task<List<PlaylistResponse>> GetUserPlaylistsAsync();

        Task<PlaylistResponse> GetByIdAsync(int playlistId);

        Task<PlaylistResponse> UpdateAsync(int playlistId, UpdatePlaylistRequest request);

        Task DeleteAsync(int playlistId);
    }
}
