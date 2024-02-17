using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class UserSubscriptionPlanRepository(ApplicationDbContext applicationDbContext)
        : Repository<UserSubscriptionPlan>(applicationDbContext), IUserSubscriptionPlanRepository
    {
        public void Update(UserSubscriptionPlan userSubscriptionPlan)
        {
            applicationDbContext.Update(userSubscriptionPlan);
        }
    }
}
