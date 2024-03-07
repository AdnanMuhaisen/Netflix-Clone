using MediatR;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Commands
{
    public class AddUserDocumentCommand(UserDocument userDocument) : IRequest<ELSAddDocumentResponse>
    {
        public readonly UserDocument userDocument = userDocument;
    }
}
