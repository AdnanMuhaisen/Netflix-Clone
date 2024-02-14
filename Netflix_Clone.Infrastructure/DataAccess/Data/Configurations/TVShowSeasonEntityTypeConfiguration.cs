using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    public class TVShowSeasonEntityTypeConfiguration : IEntityTypeConfiguration<TVShowSeason>
    {
        public void Configure(EntityTypeBuilder<TVShowSeason> builder)
        {
            builder.ToTable("tbl_TVShowSeason")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.SeasonName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.DirectoryName)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.HasMany(x => x.Episodes)
                .WithOne(x => x.Season)
                .HasForeignKey(x => x.SeasonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
