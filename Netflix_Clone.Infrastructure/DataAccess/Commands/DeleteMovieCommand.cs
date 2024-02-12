using MediatR;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class DeleteMovieCommand(int ContentId) : IRequest<bool>
    {
        public readonly int contentId = ContentId;
    }
}
