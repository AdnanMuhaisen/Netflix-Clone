using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteFromUserWatchListCommand(string UserId, int ContentId) : IRequest<ApiResponseDto>
    {
        public readonly string userId = UserId;
        public readonly int contentId = ContentId;
    }
}
