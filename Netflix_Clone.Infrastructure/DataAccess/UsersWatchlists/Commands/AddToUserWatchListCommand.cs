using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Commands
{
    public class AddToUserWatchListCommand(string UserId, int ContentId) 
        : IRequest<ApiResponseDto<bool>>
    {
        public readonly string userId = UserId;
        public readonly int contentId = ContentId;
    }
}
