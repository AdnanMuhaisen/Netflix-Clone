using MediatR;
using Microsoft.AspNetCore.Identity;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Commands;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class AssignUserToRoleCommandHandler : IRequestHandler<AssignUserToRoleCommand, ApiResponseDto>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AssignUserToRoleCommandHandler(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ApiResponseDto> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.assignUserToRoleRequestDto.UserId);
            if (user is null)
            {
                return new ApiResponseDto
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = $"The user with id {request.assignUserToRoleRequestDto.UserId} does not exist"
                };
            }

            if(!await roleManager.RoleExistsAsync(request.assignUserToRoleRequestDto.RoleName))
            {
                return new ApiResponseDto
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = $"The role with name {request.assignUserToRoleRequestDto.RoleName} does not exist"
                };
            }

            if(await userManager.IsInRoleAsync(user, request.assignUserToRoleRequestDto.RoleName))
            {
                return new ApiResponseDto
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = $"The user with id {request.assignUserToRoleRequestDto.UserId} Is already in the role : {request.assignUserToRoleRequestDto.RoleName}"
                };
            }

            // assign the user to the role:
            var result = await userManager.AddToRoleAsync(user, request.assignUserToRoleRequestDto.RoleName);

            return result.Succeeded
                ? new ApiResponseDto
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = true },
                    Message = string.Empty
                }

                : new ApiResponseDto
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = string.Join(',', result.Errors.Select(x => x.Description))
                };
        }
    }
}
