using PlaylistApi.Application.DTOs.Songs;
using System.ComponentModel;

namespace PlaylistApi.Application.Interfaces.Services
{
    public interface ISongService
    {
        Task<List<SongSearchResponse>> SearchAsync(string query);

        Task<SongSearchResponse> AddToPlaylistAsync(int playlistId, AddSongRequest request);

        Task DeleteFromPlaylistAsync(int playlistId, int songId);
    }
}
