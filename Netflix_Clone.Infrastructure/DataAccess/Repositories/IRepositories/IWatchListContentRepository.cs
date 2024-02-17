using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IWatchListContentRepository : IRepository<WatchListContent>
    {
        void Update(WatchListContent watchListContent);
    }
}
