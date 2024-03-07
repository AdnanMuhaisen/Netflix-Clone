using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands
{
    public class UpdateMovieDocumentCommand(MovieDto movieDto) : IRequest<ELSUpdateDocumentResponse>
    {
        public readonly MovieDto movieDto = movieDto;
    }
}
