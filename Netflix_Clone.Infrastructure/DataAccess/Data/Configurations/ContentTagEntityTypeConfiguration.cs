using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class ContentTagEntityTypeConfiguration : IEntityTypeConfiguration<ContentTag>
    {
        public void Configure(EntityTypeBuilder<ContentTag> builder)
        {
            builder.ToTable("tbl_ContentsTags")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
