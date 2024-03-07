using MediatR;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Handlers
{
    internal class AddUserDocumentCommandHandler(IUsersIndexRepository usersIndexRepository) : IRequestHandler<AddUserDocumentCommand, ELSAddDocumentResponse>
    {
        private readonly IUsersIndexRepository usersIndexRepository = usersIndexRepository;

        public async Task<ELSAddDocumentResponse> Handle(AddUserDocumentCommand request, CancellationToken cancellationToken)
         => await usersIndexRepository.IndexDocumentAsync(request.userDocument);
    }
}
