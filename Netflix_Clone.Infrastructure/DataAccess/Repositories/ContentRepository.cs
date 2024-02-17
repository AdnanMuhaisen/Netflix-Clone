using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class ContentRepository(ApplicationDbContext applicationDbContext)
        : Repository<Content>(applicationDbContext), IContentRepository
    {
        public void Update(Content content)
        {
            applicationDbContext.Update(content);
        }
    }
}
