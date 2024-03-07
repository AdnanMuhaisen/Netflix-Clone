using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands
{
    public class DeleteMovieDocumentCommand(int contentId) : IRequest<ELSDeleteDocumentResponse>
    {
        public readonly int contentId = contentId;
    }
}
