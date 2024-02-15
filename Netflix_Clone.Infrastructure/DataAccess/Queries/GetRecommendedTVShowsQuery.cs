using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetRecommendedTVShowsQuery(string UserId,int TotalNumberOfItemsRetrieved) : IRequest<ApiResponseDto>
    {
        public readonly string userId = UserId;
        public readonly int totalNumberOfItemsRetrieved = TotalNumberOfItemsRetrieved;
    }
}
