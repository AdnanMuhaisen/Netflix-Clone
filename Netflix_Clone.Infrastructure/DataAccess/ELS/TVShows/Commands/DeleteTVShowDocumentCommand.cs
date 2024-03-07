using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Commands
{
    public class DeleteTVShowDocumentCommand(int tvShowId) : IRequest<ELSDeleteDocumentResponse>
    {
        public readonly int tvShowId = tvShowId;
    }
}
