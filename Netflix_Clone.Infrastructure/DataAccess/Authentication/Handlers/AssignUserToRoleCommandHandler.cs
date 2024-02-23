using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;
using Serilog.Core;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Handlers
{
    public class AssignUserToRoleCommandHandler(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AssignUserToRoleCommandHandler> logger) 
        : IRequestHandler<AssignUserToRoleCommand, ApiResponseDto<AssignUserToRoleResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly ILogger<AssignUserToRoleCommandHandler> logger = logger;

        public async Task<ApiResponseDto<AssignUserToRoleResponseDto>> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.assignUserToRoleRequestDto.UserId);

            if (user is null)
            {
                logger.LogTrace($"Can not find the user with id : {request.assignUserToRoleRequestDto.UserId}");

                return new ApiResponseDto<AssignUserToRoleResponseDto>
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = $"The user with id {request.assignUserToRoleRequestDto.UserId} does not exist",
                    IsSucceed = false
                };
            }

            if (!await roleManager.RoleExistsAsync(request.assignUserToRoleRequestDto.RoleName))
            {
                logger.LogTrace($"Can not find the role with name : {request.assignUserToRoleRequestDto.RoleName}");

                return new ApiResponseDto<AssignUserToRoleResponseDto>
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = $"The role with name {request.assignUserToRoleRequestDto.RoleName} does not exist",
                    IsSucceed = false
                };
            }

            if (await userManager.IsInRoleAsync(user, request.assignUserToRoleRequestDto.RoleName))
            {
                logger.LogTrace($"The user with id : {request.assignUserToRoleRequestDto.UserId} is already" +
                    $"have the role : {request.assignUserToRoleRequestDto.RoleName}");

                return new ApiResponseDto<AssignUserToRoleResponseDto>
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = $"The user with id {request.assignUserToRoleRequestDto.UserId} Is already in the role : {request.assignUserToRoleRequestDto.RoleName}",
                    IsSucceed = false
                };
            }

            // assign the user to the role:
            var result = await userManager.AddToRoleAsync(user, request.assignUserToRoleRequestDto.RoleName);

            if (result.Succeeded)
            {
                logger.LogInformation($"The user with id : {request.assignUserToRoleRequestDto.UserId} is " +
                    $"assigned to the role : {request.assignUserToRoleRequestDto.RoleName} successfully");
            }
            else
            {
                logger.LogInformation($"Can not assign the user with id : {request.assignUserToRoleRequestDto.UserId}" +
                    $"to the role : {request.assignUserToRoleRequestDto.RoleName} due to : " +
                    $"{string.Join(',', result.Errors.Select(x => x.Description))}");
            }

            return result.Succeeded
                ? new ApiResponseDto<AssignUserToRoleResponseDto>
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = true },
                    Message = string.Empty,
                    IsSucceed = true
                }

                : new ApiResponseDto<AssignUserToRoleResponseDto>
                {
                    Result = new AssignUserToRoleResponseDto { IsAssigned = false },
                    Message = string.Join(',', result.Errors.Select(x => x.Description)),
                    IsSucceed= false
                };
        }
    }
}
