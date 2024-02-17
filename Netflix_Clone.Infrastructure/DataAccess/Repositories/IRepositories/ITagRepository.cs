using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        void Update(Tag tag);
    }
}
