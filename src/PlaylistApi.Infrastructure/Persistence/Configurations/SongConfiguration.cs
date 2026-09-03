using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaylistApi.Domain.Entities;

namespace PlaylistApi.Infrastructure.Persistence.Configurations
{
    public class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ExternalId).IsRequired();

            builder.Property(s => s.Title).IsRequired().HasMaxLength(255);

            builder.Property(s => s.ArtistName).IsRequired().HasMaxLength(255);

            builder.Property(s => s.AlbumName).HasMaxLength(255);

            builder.Property(s => s.DurationSeconds);

            builder.Property(s => s.ArtworkUrl).HasMaxLength(1000);

            builder.Property(s => s.ExternalUrl).HasMaxLength(1000);

            builder.HasIndex(s => s.ExternalId).IsUnique();
        }
    }
}
