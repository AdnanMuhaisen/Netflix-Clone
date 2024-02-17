using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface ITVShowRepository : IRepository<TVShow>
    {
        void Update(TVShow TVShow);
    }
}
