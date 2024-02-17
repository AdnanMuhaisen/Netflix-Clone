using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface ITVShowSeasonRepository : IRepository<TVShowSeason>
    {
        void Update(TVShowSeason TVShowSeason);
    }
}
