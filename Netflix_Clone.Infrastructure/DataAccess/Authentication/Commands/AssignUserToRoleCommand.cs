using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands
{
    public class AssignUserToRoleCommand(AssignUserToRoleRequestDto assignUserToRoleRequestDto)
        : IRequest<ApiResponseDto<AssignUserToRoleResponseDto>>
    {
        public readonly AssignUserToRoleRequestDto assignUserToRoleRequestDto = assignUserToRoleRequestDto;
    }
}
