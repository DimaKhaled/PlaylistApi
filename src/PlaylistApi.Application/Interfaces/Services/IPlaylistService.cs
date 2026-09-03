namespace PlaylistApi.Application.Interfaces.Services
{
    public interface IPlaylistService
    {
        Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request);

        Task<List<PlaylistResponse>> GetUserPlaylists();

        Task<PlaylistResponse> GetByIdAsync(int playlistId);

        Task<PlaylistResponse> UpdateAsync(int playlistId, UpdatePlaylistRequest request);

        Task DeleteAsync(int playlistId);
    }
}
