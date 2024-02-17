using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IContentLanguageRepository : IRepository<ContentLanguage>
    {
        void Update(ContentLanguage contentLanguage);
    }
}
