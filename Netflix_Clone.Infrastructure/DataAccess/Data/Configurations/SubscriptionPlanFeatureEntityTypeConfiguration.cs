using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class SubscriptionPlanFeatureEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionPlanFeature>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlanFeature> builder)
        {
            builder.ToTable("tbl_SubscriptionPlanFeatures")
                .HasKey(t => t.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Feature)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
