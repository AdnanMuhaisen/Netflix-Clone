using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class WatchListContentRepository(ApplicationDbContext applicationDbContext) 
        : Repository<WatchListContent>(applicationDbContext), IWatchListContentRepository
    {
        public void Update(WatchListContent watchListContent)
        {
            applicationDbContext.Update(watchListContent);
        }
    }
}
