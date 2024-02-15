using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class AddNewRoleCommand(string RoleName) : IRequest<ApiResponseDto>
    {
        public readonly string roleName = RoleName;
    }
}
