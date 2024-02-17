using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class SubscriptionPlanFeatureRepository(ApplicationDbContext applicationDbContext)
        : Repository<SubscriptionPlanFeature>(applicationDbContext), ISubscriptionPlanFeatureRepository
    {
        public void Update(SubscriptionPlanFeature subscriptionPlanFeature)
        {
            applicationDbContext.Update(subscriptionPlanFeature);
        }
    }
}
