using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Queries
{
    public class GetRecommendedMoviesQuery(string UserId, int TotalNumberOfItemsRetrieved)
        : IRequest<ApiResponseDto<IEnumerable<MovieDto>>>
    {
        public readonly string userId = UserId;
        public readonly int totalNumberOfItemsRetrieved = TotalNumberOfItemsRetrieved;
    }
}
