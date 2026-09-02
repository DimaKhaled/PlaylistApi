namespace PlaylistApi.Domain.Entities
{
    public class Playlist
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; } 

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
