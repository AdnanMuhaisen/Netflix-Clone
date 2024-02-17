using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Common.Commands
{
    public class AddToUserWatchHistoryCommand(AddToUserWatchHistoryRequestDto addToUserWatchHistoryRequestDto)
        : IRequest<ApiResponseDto<bool>>
    {
        public readonly AddToUserWatchHistoryRequestDto addToUserWatchHistoryRequestDto = addToUserWatchHistoryRequestDto;
    }
}
