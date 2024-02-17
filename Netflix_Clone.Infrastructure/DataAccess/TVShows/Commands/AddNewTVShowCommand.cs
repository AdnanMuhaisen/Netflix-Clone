using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands
{
    public class AddNewTVShowCommand(TVShowToInsertDto tVShowToInsertDto) : IRequest<ApiResponseDto>
    {
        public readonly TVShowToInsertDto tVShowToInsertDto = tVShowToInsertDto;
    }
}
