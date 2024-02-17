using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IContentTagRepository : IRepository<ContentTag>
    {
        void Update(ContentTag contentTag);
    }
}
