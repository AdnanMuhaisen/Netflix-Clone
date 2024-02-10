using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class ContentLanguageEntityTypeConfiguration : IEntityTypeConfiguration<ContentLanguage>
    {
        public void Configure(EntityTypeBuilder<ContentLanguage> builder)
        {
            builder.ToTable("tbl_ContentLanguages")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Language)
                .HasMaxLength(40)
                .IsRequired();
        }
    }
}
