using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Handlers
{
    public class SearchByUserNameQueryHandler(IUsersIndexRepository usersIndexRepository)
                : IRequestHandler<SearchByUserNameQuery, ELSSearchResponse<UserDocument>>
    {
        private readonly IUsersIndexRepository usersIndexRepository = usersIndexRepository;

        public async Task<ELSSearchResponse<UserDocument>> Handle(SearchByUserNameQuery request, CancellationToken cancellationToken)
            => await usersIndexRepository.SearchByUserNameAsync(request.searchQuery);
    }
}
