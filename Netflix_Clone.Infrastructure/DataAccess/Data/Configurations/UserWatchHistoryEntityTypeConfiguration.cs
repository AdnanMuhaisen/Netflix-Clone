using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class UserWatchHistoryEntityTypeConfiguration : IEntityTypeConfiguration<UserWatchHistory>
    {
        public void Configure(EntityTypeBuilder<UserWatchHistory> builder)
        {
            builder.ToTable("tbl_UsersWatchHistory")
                .HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
