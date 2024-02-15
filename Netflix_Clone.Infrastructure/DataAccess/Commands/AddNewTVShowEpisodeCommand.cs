using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewTVShowEpisodeCommand(TVShowEpisodeToInsertDto TVShowEpisodeToInsertDto)
        : IRequest<ApiResponseDto>
    {
        public TVShowEpisodeToInsertDto TVShowEpisodeToInsertDto { get; } = TVShowEpisodeToInsertDto;
    }
}
