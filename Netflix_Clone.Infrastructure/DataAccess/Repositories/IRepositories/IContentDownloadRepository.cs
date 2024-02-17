using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IContentDownloadRepository : IRepository<ContentDownload>
    {
        void Update(ContentDownload contentDownload);
    }
}
