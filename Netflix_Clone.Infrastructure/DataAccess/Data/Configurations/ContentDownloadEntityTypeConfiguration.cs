using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    public class ContentDownloadEntityTypeConfiguration : IEntityTypeConfiguration<ContentDownload>
    {
        public void Configure(EntityTypeBuilder<ContentDownload> builder)
        {
            builder.ToTable("tbl_UsersDownloads")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
