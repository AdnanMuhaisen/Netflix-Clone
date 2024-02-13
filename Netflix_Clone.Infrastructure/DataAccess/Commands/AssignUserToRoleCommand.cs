using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AssignUserToRoleCommand(AssignUserToRoleRequestDto assignUserToRoleRequestDto) 
        : IRequest<AssignUserToRoleResponseDto>
    {
        public readonly AssignUserToRoleRequestDto assignUserToRoleRequestDto = assignUserToRoleRequestDto;
    }
}
