using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class TVShowRepository(ApplicationDbContext applicationDbContext)
        : Repository<TVShow>(applicationDbContext), ITVShowRepository
    {
        public void Update(TVShow TVShow)
        {
            applicationDbContext.Update(TVShow);
        }
    }
}
