using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteSeasonEpisodeCommand(TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto)
        : IRequest<ApiResponseDto>
    {
        public readonly TVShowSeasonEpisodeToDeleteDto tVShowSeasonEpisodeToDeleteDto = tVShowSeasonEpisodeToDeleteDto;
    }
}
