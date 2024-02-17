using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class UserWatchListRepository(ApplicationDbContext applicationDbContext)
        : Repository<UserWatchList>(applicationDbContext), IUserWatchListRepository
    {
        public void Update(UserWatchList userWatchList)
        {
            applicationDbContext.Update(userWatchList);
        }
    }
}
