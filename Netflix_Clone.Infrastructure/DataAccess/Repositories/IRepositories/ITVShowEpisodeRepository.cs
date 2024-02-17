using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface ITVShowEpisodeRepository : IRepository<TVShowEpisode>
    {
        void Update(TVShowEpisode episode);
    }
}
