using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Commands
{
    public class AddNewTVShowSeasonCommand(TVShowSeasonToInsertDto tVShowSeasonToInsertDto)
        : IRequest<ApiResponseDto>
    {
        public readonly TVShowSeasonToInsertDto tVShowSeasonToInsertDto = tVShowSeasonToInsertDto;
    }
}
