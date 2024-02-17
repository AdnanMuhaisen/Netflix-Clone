using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
    {
        void Update(SubscriptionPlan subscriptionPlan);
    }
}
