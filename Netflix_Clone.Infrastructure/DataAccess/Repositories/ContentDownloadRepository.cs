using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class ContentDownloadRepository(ApplicationDbContext applicationDbContext)
        : Repository<ContentDownload>(applicationDbContext), IContentDownloadRepository
    {
        public void Update(ContentDownload contentDownload)
        {
            applicationDbContext.Update(contentDownload);
        }
    }
}
