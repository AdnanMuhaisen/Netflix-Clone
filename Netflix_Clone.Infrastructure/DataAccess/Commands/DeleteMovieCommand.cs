using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteMovieCommand(int ContentId) : IRequest<ApiResponseDto>
    {
        public readonly int contentId = ContentId;
    }
}
