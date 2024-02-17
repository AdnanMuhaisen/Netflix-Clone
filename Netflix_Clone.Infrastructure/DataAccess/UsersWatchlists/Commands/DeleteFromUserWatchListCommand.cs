using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.UsersWatchlists.Commands
{
    public class DeleteFromUserWatchListCommand(string UserId, int ContentId) 
        : IRequest<ApiResponseDto<DeletionResultDto>>
    {
        public readonly string userId = UserId;
        public readonly int contentId = ContentId;
    }
}
