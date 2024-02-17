using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface ISubscriptionPlanFeatureRepository : IRepository<SubscriptionPlanFeature>
    {
        void Update(SubscriptionPlanFeature subscriptionPlanFeature);
    }
}
