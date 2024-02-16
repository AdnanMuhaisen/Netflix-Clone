using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class WatchListContentEntityTypeConfiguration : IEntityTypeConfiguration<WatchListContent>
    {
        public void Configure(EntityTypeBuilder<WatchListContent> builder)
        {
            builder.ToTable("tbl_WatchListsContents")
                .HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
