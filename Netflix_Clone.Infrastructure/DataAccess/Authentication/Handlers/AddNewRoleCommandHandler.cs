using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Handlers
{
    public class AddNewRoleCommandHandler(
        ILogger<AddNewRoleCommandHandler> logger,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<AddNewRoleCommand, ApiResponseDto>
    {
        private readonly ILogger<AddNewRoleCommandHandler> logger = logger;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;

        public async Task<ApiResponseDto> Handle(AddNewRoleCommand request, CancellationToken cancellationToken)
        {
            if (await roleManager.RoleExistsAsync(request.roleName))
            {
                return new ApiResponseDto
                {
                    Result = new AddNewRoleResponseDto
                    {
                        IsAdded = false,
                    },
                    Message = $"The role with the name {request.roleName} is already exist"
                };
            }

            //add the role 

            var result = await roleManager.CreateAsync(new IdentityRole
            {
                Name = request.roleName,
                NormalizedName = request.roleName.ToUpper(),
            });

            return result.Succeeded
                ? new ApiResponseDto
                {
                    Result = new AddNewRoleResponseDto { IsAdded = true },
                    Message = string.Empty
                }
                : new ApiResponseDto
                {
                    Result = new AddNewRoleResponseDto { IsAdded = false },
                    Message = string.Join(',', result.Errors.Select(x => x.Description))
                };
        }
    }
}
