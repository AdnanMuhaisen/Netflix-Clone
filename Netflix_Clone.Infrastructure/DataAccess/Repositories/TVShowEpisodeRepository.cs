using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class TVShowEpisodeRepository(ApplicationDbContext applicationDbContext) 
        : Repository<TVShowEpisode>(applicationDbContext), ITVShowEpisodeRepository
    {
        public void Update(TVShowEpisode episode)
        {
            applicationDbContext.Update(episode);
        }
    }
}
