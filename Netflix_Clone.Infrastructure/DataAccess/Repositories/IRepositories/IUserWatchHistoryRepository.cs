using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IUserWatchHistoryRepository : IRepository<UserWatchHistory>
    {
        void Update(UserWatchHistory userWatchHistory);
    }
}
