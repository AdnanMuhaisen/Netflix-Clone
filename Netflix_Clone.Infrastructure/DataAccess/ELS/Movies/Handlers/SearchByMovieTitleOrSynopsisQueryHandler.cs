using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Queries;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Handlers
{
    public class SearchByMovieTitleOrSynopsisQueryHandler(IMoviesIndexRepository moviesIndexRepository)
        : IRequestHandler<SearchByMovieTitleOrSynopsisQuery, ELSSearchResponse<MovieDocument>>
    {
        private readonly IMoviesIndexRepository moviesIndexRepository = moviesIndexRepository;

        public async Task<ELSSearchResponse<MovieDocument>> Handle(SearchByMovieTitleOrSynopsisQuery request, CancellationToken cancellationToken)
          => await moviesIndexRepository.SearchByMovieTitleOrSynopsisAsync(request.searchQuery);
        
    }
}
