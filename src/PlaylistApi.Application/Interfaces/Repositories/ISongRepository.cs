using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Application.Interfaces.Repositories
{
    public interface ISongRepository
    {
        Task<Song?> GetByExternalIdAsync(long externalId);

        Task AddAsync(Song song);

        Task AddToPlaylistAsync(PlaylistSong playlistSong);

        Task<bool> IsSongInPlaylist(int playlistId, int songId);

        Task DeleteFromPlaylist(int playlistId, int songId);
    }
}
