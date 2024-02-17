using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class ContentGenreRepository(ApplicationDbContext applicationDbContext)
        : Repository<ContentGenre>(applicationDbContext), IContentGenreRepository
    {
        public void Update(ContentGenre contentGenre)
        {
            applicationDbContext.Update(contentGenre);
        }
    }
}
