using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Handlers
{
    public class SearchByTVShowTitleOrSynopsisQueryHandler(ITVShowsIndexRepository tvShowsIndexRepository) : IRequestHandler<SearchByTVShowTitleOrSynopsisQuery, ELSSearchResponse<TVShowDocument>>
    {
        public readonly ITVShowsIndexRepository tvShowsIndexRepository = tvShowsIndexRepository;

        public async Task<ELSSearchResponse<TVShowDocument>> Handle(SearchByTVShowTitleOrSynopsisQuery request, CancellationToken cancellationToken)
            => await tvShowsIndexRepository.SearchByTVShowTitleOrSynopsisAsync(request.searchQuery);
    }
}
