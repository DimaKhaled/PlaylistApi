using System.ComponentModel;

namespace PlaylistApi.Application.Interfaces.Services
{
    public interface ISongService
    {
        Task<List<SongSearchResponse>> SearchAsync(SongSearchRequest request);

        Task<SongResponse> AddToPlaylistAsync(int playlistId, AddSongRequest request);

        Task DeleteFromPlaylistAsync(int playlistId, int songId);
    }
}
