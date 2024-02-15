using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AssignUserToRoleCommand(AssignUserToRoleRequestDto assignUserToRoleRequestDto) 
        : IRequest<ApiResponseDto>
    {
        public readonly AssignUserToRoleRequestDto assignUserToRoleRequestDto = assignUserToRoleRequestDto;
    }
}
