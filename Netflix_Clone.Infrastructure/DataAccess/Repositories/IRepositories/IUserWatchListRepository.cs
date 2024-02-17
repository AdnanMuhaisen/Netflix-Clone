using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IUserWatchListRepository : IRepository<UserWatchList>
    {
        void Update(UserWatchList userWatchList);
    }
}
