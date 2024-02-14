using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteTVShowSeasonCommand(DeleteTVShowSeasonRequestDto deleteTVShowSeasonRequestDto) : IRequest<DeletionResultDto>
    {
        public readonly DeleteTVShowSeasonRequestDto deleteTVShowSeasonRequestDto = deleteTVShowSeasonRequestDto;
    }
}
