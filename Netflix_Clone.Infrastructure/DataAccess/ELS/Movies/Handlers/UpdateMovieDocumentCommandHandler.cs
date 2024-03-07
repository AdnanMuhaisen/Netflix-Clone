using Mapster;
using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Handlers
{
    internal class UpdateMovieDocumentCommandHandler(IMoviesIndexRepository moviesIndexRepository) 
        : IRequestHandler<UpdateMovieDocumentCommand, ELSUpdateDocumentResponse>
    {
        private readonly IMoviesIndexRepository moviesIndexRepository = moviesIndexRepository;

        public async Task<ELSUpdateDocumentResponse> Handle(UpdateMovieDocumentCommand request, CancellationToken cancellationToken)
            => await moviesIndexRepository.UpdateDocumentAsync(request.movieDto.Adapt<MovieDocument>());
    }
}
