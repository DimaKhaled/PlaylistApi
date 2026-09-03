using PlaylistApi.Application.Interfaces.Repositories;
using PlaylistApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PlaylistApi.Infrastructure.Persistence.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly AppDbContext _context;

        public SongRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Song?> GetByExternalIdAsync(long externalId)
        {
            return await _context.Songs.FirstOrDefaultAsync(s => s.ExternalId == externalId);
        }


        public async Task AddAsync(Song song)
        {
            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
        }


        public async Task AddToPlaylistAsync(PlaylistSong playlistSong)
        {
            await _context.PlaylistSongs.AddAsync(playlistSong);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> IsSongInPlaylistAsync(int playlistId, int songId)
        {
            return await _context.PlaylistSongs.AnyAsync(ps =>
                    ps.PlaylistId == playlistId && ps.SongId == songId);
        }


        public async Task DeleteFromPlaylistAsync(int playlistId, int songId)
        {
            var playlistSong = await _context.PlaylistSongs.FirstOrDefaultAsync(ps =>
                    ps.PlaylistId == playlistId && ps.SongId == songId);

            if (playlistSong is null)
            {
                return;
            }

            _context.PlaylistSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();
        }
    }
}
