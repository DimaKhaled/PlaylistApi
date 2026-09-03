using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaylistApi.Domain.Entities;
using PlaylistApi.Infrastructure.Identity;

namespace PlaylistApi.Infrastructure.Persistence.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.Property(p => p.Description).HasMaxLength(500);

            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasOne<ApplicationUser>().WithMany(u => u.playlists)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId);
        }
    }
}
