using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class ContentAwardEntityTypeConfiguration : IEntityTypeConfiguration<ContentAward>
    {
        public void Configure(EntityTypeBuilder<ContentAward> builder)
        {
            builder.ToTable("tbl_ContentsAwards")
                .HasKey(x => x.Id);
        }
    }
}
