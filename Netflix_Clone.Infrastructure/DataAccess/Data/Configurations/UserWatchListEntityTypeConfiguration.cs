using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class UserWatchListEntityTypeConfiguration : IEntityTypeConfiguration<UserWatchList>
    {
        public void Configure(EntityTypeBuilder<UserWatchList> builder)
        {
            builder.ToTable("tbl_UsersWatchLists")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();


            builder.HasOne(x => x.ApplicationUser)
                .WithOne(x => x.WatchList)
                .HasForeignKey<UserWatchList>(x => x.UserId)
                .IsRequired(true);

            builder.HasMany(x => x.WatchListContents)
                .WithMany(x => x.ExistsInWatchLists)
                .UsingEntity<WatchListContent>(
                l => l.HasOne(x => x.Content).WithMany(x => x.WatchListsContents).HasForeignKey(x => x.ContentId).IsRequired(),
                r => r.HasOne(x => x.WatchList).WithMany(x => x.WatchListsContents).HasForeignKey(x => x.WatchListId).IsRequired()
                );
        }
    }
}
