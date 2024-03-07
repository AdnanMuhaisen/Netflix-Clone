using Mapster;
using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.TVShows.Handlers
{
    public class AddNewTVShowDocumentCommandHandler(ITVShowsIndexRepository tvShowsIndexRepository)
        : IRequestHandler<AddNewTVShowDocumentCommand, ELSAddDocumentResponse>
    {
        private readonly ITVShowsIndexRepository tvShowsIndexRepository = tvShowsIndexRepository;

        public Task<ELSAddDocumentResponse> Handle(AddNewTVShowDocumentCommand request, CancellationToken cancellationToken)
            => tvShowsIndexRepository.IndexDocumentAsync(request.tvShowDto.Adapt<TVShowDocument>());
    }
}
