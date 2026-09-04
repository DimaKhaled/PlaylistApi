using PlaylistApi.Application.DTOs.Playlists;
using PlaylistApi.Application.Exceptions;
using PlaylistApi.Application.Interfaces.Authentication;
using PlaylistApi.Application.Interfaces.Repositories;
using PlaylistApi.Application.Interfaces.Services;
using PlaylistApi.Application.Mappings;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ICurrentUserService _currentUserService;


        public PlaylistService(IPlaylistRepository playlistRepository, ICurrentUserService currentUserService)
        {
            _playlistRepository = playlistRepository;
            _currentUserService = currentUserService;
        }


        public async Task<PlaylistResponse> CreateAsync(CreatePlaylistRequest request)
        {
            var userId = _currentUserService.UserId;

            var playlist = new Playlist
            {
                UserId = userId,
                Name = request.Name.Trim(),
                Description = TrimDescription(request.Description),
                CreatedAt = DateTime.Now
            };
            await _playlistRepository.AddAsync(playlist);
            return playlist.ToResponse();
        }



        public async Task<List<PlaylistResponse>> GetUserPlaylistsAsync()
        {
            var userId = _currentUserService.UserId;
            var playlists = await _playlistRepository.GetUserPlaylistsAsync(userId);
            return playlists.Select(p => p.ToResponse()).ToList();
        }



        public async Task<PlaylistResponse> GetByIdAsync(int playlistId)
        {
            var userId = _currentUserService.UserId;
            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, userId);
            if (playlist == null)
            {
                throw new NotFoundException("Playlist was not found.");
            }
            return playlist.ToResponse();
        }


         
        public async Task<PlaylistResponse> UpdateAsync(int playlistId, UpdatePlaylistRequest request)
        {
            var userId = _currentUserService.UserId;

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, userId);
            if (playlist == null)
            {
                throw new NotFoundException("Playlist was not found.");
            }
            playlist.Name = request.Name.Trim();
            playlist.Description = TrimDescription(request.Description);

            await _playlistRepository.UpdateAsync(playlist);
            return playlist.ToResponse();
        }



        public async Task DeleteAsync(int playlistId)
        {
            var userId = _currentUserService.UserId;

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, userId);
            if (playlist == null)
            {
                throw new NotFoundException("Playlist was not found.");
            }
            await _playlistRepository.DeleteAsync(playlist);
        }



        private static string? TrimDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return null;
            return description.Trim();
        }
    }
}
