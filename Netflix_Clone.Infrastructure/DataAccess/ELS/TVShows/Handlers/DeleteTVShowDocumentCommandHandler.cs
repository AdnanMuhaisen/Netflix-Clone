using MediatR;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Handlers
{
    public class DeleteTVShowDocumentCommandHandler(ITVShowsIndexRepository tvShowsIndexRepository)
        : IRequestHandler<DeleteTVShowDocumentCommand, ELSDeleteDocumentResponse>
    {
        private readonly ITVShowsIndexRepository tvShowsIndexRepository = tvShowsIndexRepository;

        public Task<ELSDeleteDocumentResponse> Handle(DeleteTVShowDocumentCommand request, CancellationToken cancellationToken)
            => tvShowsIndexRepository.DeleteDocumentAsync(request.tvShowId);
    }
}
