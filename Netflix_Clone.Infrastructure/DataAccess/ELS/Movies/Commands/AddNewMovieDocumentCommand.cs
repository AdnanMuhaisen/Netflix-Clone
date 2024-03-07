using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands
{
    public class AddNewMovieDocumentCommand(MovieDto movieDto) : IRequest<ELSAddDocumentResponse>
    {
        public readonly MovieDto movieDto = movieDto;
    }
}
