using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class ContentLanguageRepository(ApplicationDbContext applicationDbContext)
        : Repository<ContentLanguage>(applicationDbContext), IContentLanguageRepository
    {
        public void Update(ContentLanguage contentLanguage)
        {
            applicationDbContext.Update(contentLanguage);
        }
    }
}
