using PlaylistApi.Application.DTOs.Playlists;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Application.Mappings
{
    public static class PlaylistMapping
    {
        public static PlaylistResponse ToResponse(this Playlist playlist)
        {
            return new PlaylistResponse
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                CreatedAt = playlist.CreatedAt,
                Songs = playlist.PlaylistSongs.Select(ps => ps.ToResponse()).ToList() 
            };
        }



        public static PlaylistSongResponse ToResponse(this PlaylistSong playlistSong)
        {
            return new PlaylistSongResponse
            {
                SongId = playlistSong.SongId,
                ExternalId = playlistSong.Song.ExternalId,
                Title = playlistSong.Song.Title,
                ArtistName = playlistSong.Song.ArtistName,
                AlbumName = playlistSong.Song.AlbumName,
                DurationSeconds = playlistSong.Song.DurationSeconds,
                ArtworkUrl = playlistSong.Song.ArtworkUrl,
                ExternalUrl = playlistSong.Song.ExternalUrl
            };
        }

    }

}
