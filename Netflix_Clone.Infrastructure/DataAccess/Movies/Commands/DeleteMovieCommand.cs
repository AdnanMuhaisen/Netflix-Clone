using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Movies.Commands
{
    public class DeleteMovieCommand(int ContentId) : IRequest<ApiResponseDto>
    {
        public readonly int contentId = ContentId;
    }
}
