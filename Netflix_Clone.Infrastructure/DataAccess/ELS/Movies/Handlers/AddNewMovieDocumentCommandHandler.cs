using Mapster;
using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Handlers
{
    public class AddNewMovieDocumentCommandHandler(IMoviesIndexRepository moviesIndexRepository)
        : IRequestHandler<AddNewMovieDocumentCommand, ELSAddDocumentResponse>
    {
        private readonly IMoviesIndexRepository moviesIndexRepository = moviesIndexRepository;

        public async Task<ELSAddDocumentResponse> Handle(AddNewMovieDocumentCommand request, CancellationToken cancellationToken)
            => await moviesIndexRepository.IndexDocumentAsync(request.movieDto.Adapt<MovieDocument>());
    }
}
