using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Commands
{
    public class AddNewTVShowDocumentCommand(TVShowDto tvShowDto) : IRequest<ELSAddDocumentResponse>
    {
        public readonly TVShowDto tvShowDto = tvShowDto;
    }
}
