using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands
{
    public class AddNewTVShowEpisodeCommand(TVShowEpisodeToInsertDto TVShowEpisodeToInsertDto)
        : IRequest<ApiResponseDto>
    {
        public TVShowEpisodeToInsertDto TVShowEpisodeToInsertDto { get; } = TVShowEpisodeToInsertDto;
    }
}
