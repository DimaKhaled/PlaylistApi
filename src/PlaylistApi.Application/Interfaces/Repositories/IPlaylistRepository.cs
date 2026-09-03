using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Application.Interfaces.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist> GetPlaylistByIdAsync(int playlistId, Guid userId);

        Task<List<Playlist>> GetUserPlaylistsAsync(Guid userId);

        Task AddAsync(Playlist playlist);

        Task UpdateAsync(Playlist playlist);

        Task DeleteAsync(Playlist playlist);
    }
}
