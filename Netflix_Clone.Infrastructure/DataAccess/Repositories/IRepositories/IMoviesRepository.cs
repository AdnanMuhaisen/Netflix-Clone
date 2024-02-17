using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IMoviesRepository : IRepository<Movie>
    {
        void Update(Movie movie);
    }
}
