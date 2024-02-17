using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class TVShowSeasonRepository(ApplicationDbContext applicationDbContext) 
        : Repository<TVShowSeason>(applicationDbContext), ITVShowSeasonRepository
    {
        public void Update(TVShowSeason TVShowSeason)
        {
            applicationDbContext.Update(TVShowSeason);
        }
    }
}
