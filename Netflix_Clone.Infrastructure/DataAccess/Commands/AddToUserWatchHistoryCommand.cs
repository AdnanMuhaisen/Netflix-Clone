using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddToUserWatchHistoryCommand(AddToUserWatchHistoryRequestDto addToUserWatchHistoryRequestDto) : IRequest<ApiResponseDto>
    {
        public readonly AddToUserWatchHistoryRequestDto addToUserWatchHistoryRequestDto = addToUserWatchHistoryRequestDto;
    }
}
