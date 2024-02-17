using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IContentRepository : IRepository<Content>
    {
        void Update(Content content);
    }
}
