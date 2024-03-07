using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Queries
{
    public class SearchByMovieTitleOrSynopsisQuery(string searchQuery) : IRequest<ELSSearchResponse<MovieDocument>>
    {
        public readonly string searchQuery = searchQuery;
    }
}
