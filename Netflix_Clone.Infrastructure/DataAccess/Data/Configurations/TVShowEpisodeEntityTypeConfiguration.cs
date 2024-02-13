using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class TVShowEpisodeEntityTypeConfiguration : IEntityTypeConfiguration<TVShowEpisode>
    {
        public void Configure(EntityTypeBuilder<TVShowEpisode> builder)
        {
            builder.ToTable("tbl_TVShowEpisodes")
                .HasKey(e => e.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Location)
                .HasMaxLength(100)
                .IsRequired(true);
        }
    }
}
