using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetUserWatchListQuery(string UserId) : IRequest<ApiResponseDto>
    {
        public readonly string userId = UserId;
    }
}
