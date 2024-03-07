using MediatR;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Movies.Handlers
{
    public class DeleteMovieDocumentCommandHandler(IMoviesIndexRepository moviesIndexRepository)
        : IRequestHandler<DeleteMovieDocumentCommand, ELSDeleteDocumentResponse>
    {
        private readonly IMoviesIndexRepository moviesIndexRepository = moviesIndexRepository;

        public async Task<ELSDeleteDocumentResponse> Handle(DeleteMovieDocumentCommand request, CancellationToken cancellationToken)
          =>  await moviesIndexRepository.DeleteDocumentAsync(request.contentId);        
    }
}
