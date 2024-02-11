using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class SubscriptionPlanEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.ToTable("tbl_SubscriptionPlans")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Plan)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(5, 2);

            #region relationships

            builder.HasMany(x => x.PlanFeatures)
                .WithMany(x => x.FeaturePlans)
                .UsingEntity<SubscriptionPlanSubscriptionPlanFeature>
                (
                r => r.HasOne(x => x.SubscriptionPlanFeature).WithMany(x => x.SubscriptionPlansFeatures).IsRequired(true),
                l => l.HasOne(x => x.SubscriptionPlan).WithMany(x => x.SubscriptionPlansFeatures).IsRequired(true)
                );

            builder.HasMany(x => x.PlanUsers)
                .WithMany(x => x.SubscriptionPlans)
                .UsingEntity<UserSubscriptionPlan>(
                l => l.HasOne(x => x.ApplicationUser).WithMany(x => x.UsersSubscriptionPlans),
                l => l.HasOne(x => x.SubscriptionPlan).WithMany(x => x.UsersSubscriptionPlans)
                );
            #endregion
        }
    }
}
