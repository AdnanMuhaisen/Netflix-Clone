using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class SubscriptionPlanSubscriptionPlanFeatureEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionPlanSubscriptionPlanFeature>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlanSubscriptionPlanFeature> builder)
        {
            // the configuration for this relationship is in the subscription plan entity

            builder.ToTable("tbl_SubscriptionPlansFeatures")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
