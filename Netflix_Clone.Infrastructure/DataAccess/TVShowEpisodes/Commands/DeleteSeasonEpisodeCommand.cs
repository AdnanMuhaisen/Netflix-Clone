using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands
{
    public class DeleteSeasonEpisodeCommand(TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto)
        : IRequest<ApiResponseDto<DeletionResultDto>>
    {
        public readonly TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto = tVShowSeasonEpisodeToDeleteDto;
    }
}
