using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Queries
{
    public class SearchByTVShowTitleOrSynopsisQuery(string searchQuery) : IRequest<ELSSearchResponse<TVShowDocument>>
    {
        public readonly string searchQuery = searchQuery;
    }
}
