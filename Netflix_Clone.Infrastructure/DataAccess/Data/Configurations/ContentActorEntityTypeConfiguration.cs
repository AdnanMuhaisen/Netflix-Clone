using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class ContentActorEntityTypeConfiguration : IEntityTypeConfiguration<ContentActor>
    {
        public void Configure(EntityTypeBuilder<ContentActor> builder)
        {
            builder.ToTable("tbl_ContentsActors")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
