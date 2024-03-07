using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Queries
{
    public class SearchByUserNameQuery(string searchQuery) : IRequest<ELSSearchResponse<UserDocument>>
    {
        public readonly string searchQuery = searchQuery;
    }
}
