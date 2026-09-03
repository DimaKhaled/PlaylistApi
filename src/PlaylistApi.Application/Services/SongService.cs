using PlaylistApi.Application.DTOs.Songs;
using PlaylistApi.Application.Exceptions;
using PlaylistApi.Application.Interfaces.Authentication;
using PlaylistApi.Application.Interfaces.ExternalServices;
using PlaylistApi.Application.Interfaces.Repositories;
using PlaylistApi.Application.Interfaces.Services;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Application.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMusicService _musicService;
        private readonly ICurrentUserService _currentUserService;

        public SongService(ISongRepository songRepository, IPlaylistRepository playlistRepository, IMusicService musicService, ICurrentUserService currentUserService)
        {
            _songRepository = songRepository;
            _playlistRepository = playlistRepository;
            _musicService = musicService;
            _currentUserService = currentUserService;
        }


        public async Task<List<SongSearchResponse>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ValidationException("Search query is required.");
            }
            return await _musicService.SearchAsync(query.Trim());
        }



        public async Task<SongSearchResponse> AddToPlaylistAsync(int playlistId, AddSongRequest request)
        {
            var userId = _currentUserService.UserId;

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, userId);
            if (playlist == null)
            {
                throw new NotFoundException("Playlist was not found.");
            }

            var song = await _songRepository.GetByExternalIdAsync(request.ExternalId);

            if (song == null)
            {
                var externalSong = await _musicService.GetByExternalIdAsync(request.ExternalId);

                if (externalSong is null)
                {
                    throw new NotFoundException("Song was not found.");
                }
                song = new Song
                {
                    ExternalId = externalSong.ExternalId,
                    Title = externalSong.Title,
                    ArtistName = externalSong.ArtistName,
                    AlbumName = externalSong.AlbumName,
                    DurationSeconds = externalSong.DurationSeconds,
                    ArtworkUrl = externalSong.ArtworkUrl,
                    ExternalUrl = externalSong.ExternalUrl
                };
                await _songRepository.AddAsync(song);
            }

            var alreadyExists = await _songRepository.IsSongInPlaylistAsync(playlistId, song.Id);
            if (alreadyExists)
            {
                throw new ConflictException("The song is already in the playlist.");
            }

            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = song.Id
            };
            await _songRepository.AddToPlaylistAsync(playlistSong);
            return new SongSearchResponse
            {
                ExternalId = song.ExternalId,
                Title = song.Title,
                ArtistName = song.ArtistName,
                AlbumName = song.AlbumName,
                DurationSeconds = song.DurationSeconds,
                ArtworkUrl = song.ArtworkUrl,
                ExternalUrl = song.ExternalUrl
            };
        }



        public async Task DeleteFromPlaylistAsync(int playlistId, int songId)
        {
            var userId = _currentUserService.UserId;

            var playlist = await _playlistRepository.GetPlaylistByIdAsync(playlistId, userId);
            if (playlist == null)
            {
                throw new NotFoundException("Playlist was not found.");
            }

            var exists = await _songRepository.IsSongInPlaylistAsync(playlistId, songId);
            if (!exists)
            {
                throw new NotFoundException("Song was not found in the playlist.");
            }
            await _songRepository.DeleteFromPlaylistAsync(playlistId, songId);
        }
    }
}
