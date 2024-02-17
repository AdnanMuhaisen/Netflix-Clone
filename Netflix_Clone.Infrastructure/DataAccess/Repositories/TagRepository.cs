using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class TagRepository(ApplicationDbContext applicationDbContext) 
        : Repository<Tag>(applicationDbContext), ITagRepository
    {
        public void Update(Tag tag)
        {
            applicationDbContext.Update(tag);
        }
    }
}
