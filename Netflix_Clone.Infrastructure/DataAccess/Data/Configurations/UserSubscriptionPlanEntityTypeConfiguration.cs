using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class UserSubscriptionPlanEntityTypeConfiguration : IEntityTypeConfiguration<UserSubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<UserSubscriptionPlan> builder)
        {
            builder.ToTable("tbl_UsersSubscriptions")
                .HasKey(t => t.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
