using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class UserWatchHistoryRepository(ApplicationDbContext applicationDbContext) 
        : Repository<UserWatchHistory>(applicationDbContext), IUserWatchHistoryRepository
    {
        public void Update(UserWatchHistory userWatchHistory)
        {
            applicationDbContext.Update(userWatchHistory);
        }
    }
}
