using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Infrastructure.Persistence.Configurations
{
    public class PlaylistSongConfiguration : IEntityTypeConfiguration<PlaylistSong>
    {
        public void Configure(EntityTypeBuilder<PlaylistSong> builder)
        {
            builder.HasKey(ps => new
            {
                ps.PlaylistId,
                ps.SongId
            });

            builder.HasOne(ps => ps.Playlist).WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.Song).WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(ps => ps.SongId);
        }
    }
}
