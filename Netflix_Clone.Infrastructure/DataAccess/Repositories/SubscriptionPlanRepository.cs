using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class SubscriptionPlanRepository(ApplicationDbContext applicationDbContext) 
        : Repository<SubscriptionPlan>(applicationDbContext), ISubscriptionPlanRepository
    {
        public void Update(SubscriptionPlan subscriptionPlan)
        {
            applicationDbContext.Update(subscriptionPlan);
        }
    }
}
