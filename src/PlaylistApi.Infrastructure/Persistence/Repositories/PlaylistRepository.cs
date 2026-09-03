using Microsoft.EntityFrameworkCore;
using PlaylistApi.Application.Interfaces.Repositories;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Infrastructure.Persistence.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;

        public PlaylistRepository(AppDbContext context) 
        {
            _context = context;
        }


        public async Task<Playlist?> GetPlaylistByIdAsync(int playlistId, Guid userId)
        {
            return await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p =>
                    p.Id == playlistId && p.UserId == userId);  
        }


        public async Task<List<Playlist>> GetUserPlaylistsAsync(Guid userId)
        {
            return await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }


        public async Task AddAsync(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Playlist playlist)
        {
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }

    }
}
