using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IUserSubscriptionPlanRepository : IRepository<UserSubscriptionPlan>
    {
        void Update(UserSubscriptionPlan userSubscriptionPlan);
    }
}
