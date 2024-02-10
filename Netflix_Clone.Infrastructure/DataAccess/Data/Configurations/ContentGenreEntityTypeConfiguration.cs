using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class ContentGenreEntityTypeConfiguration : IEntityTypeConfiguration<ContentGenre>
    {
        public void Configure(EntityTypeBuilder<ContentGenre> builder)
        {
            builder.ToTable("tbl_ContentGenres")
                .HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
