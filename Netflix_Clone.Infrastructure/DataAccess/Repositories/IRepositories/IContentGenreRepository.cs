using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IContentGenreRepository : IRepository<ContentGenre>
    {
        void Update(ContentGenre contentGenre);
    }
}
