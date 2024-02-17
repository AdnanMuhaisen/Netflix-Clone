using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class ContentTagRepository(ApplicationDbContext applicationDbContext)
        : Repository<ContentTag>(applicationDbContext), IContentTagRepository
    {
        public void Update(ContentTag contentTag)
        {
            applicationDbContext.Update(contentTag);
        }
    }
}
