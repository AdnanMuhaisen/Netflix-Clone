using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Queries
{
    public class GetUserWatchListQuery(string UserId) : IRequest<ApiResponseDto>
    {
        public readonly string userId = UserId;
    }
}
