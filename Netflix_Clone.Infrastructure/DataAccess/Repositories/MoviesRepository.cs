using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class MoviesRepository(ApplicationDbContext applicationDbContext) 
        : Repository<Movie>(applicationDbContext), IMoviesRepository
    {
        public void Update(Movie movie)
        {
            applicationDbContext.Update(movie);
        }
    }
}
