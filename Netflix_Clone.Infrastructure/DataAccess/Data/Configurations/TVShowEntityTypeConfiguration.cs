using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class TVShowEntityTypeConfiguration : IEntityTypeConfiguration<TVShow>
    {
        public void Configure(EntityTypeBuilder<TVShow> builder)
        {
            //builder.ToTable("tbl_TVShows");

            //builder.Property(e => e.Id)
            //    .ValueGeneratedOnAdd();

            //relationships
            builder.HasMany(x => x.Seasons)
                .WithOne(x => x.TVShow)
                .HasForeignKey(x => x.TVShowId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
