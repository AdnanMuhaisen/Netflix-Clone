using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Handlers
{
    public class AddNewRoleCommandHandler(
        ILogger<AddNewRoleCommandHandler> logger,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<AddNewRoleCommand, ApiResponseDto<AddNewRoleResponseDto>>
    {
        private readonly ILogger<AddNewRoleCommandHandler> logger = logger;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;

        public async Task<ApiResponseDto<AddNewRoleResponseDto>> Handle(AddNewRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.roleName))
            {
                logger.LogError($"Can not add the role with role name : {request.roleName} because it is null " +
                    $"or empty");

                return new ApiResponseDto<AddNewRoleResponseDto>
                {
                    Result = new AddNewRoleResponseDto
                    {
                        IsAdded = false
                    },
                    Message = $"Invalid Role Name: {request.roleName}",
                    IsSucceed = false
                };
            }

            if (await roleManager.RoleExistsAsync(request.roleName))
            {
                logger.LogError($"The role with name : {request.roleName} is already exist");

                return new ApiResponseDto<AddNewRoleResponseDto>
                {
                    Result = new AddNewRoleResponseDto
                    {
                        IsAdded = false
                    },
                    Message = $"The role with the name {request.roleName} is already exist",
                    IsSucceed = false
                };
            }

            //add the role 
            try
            {
                logger.LogTrace($"Try to save the Role : {request.roleName} in the database");

                var result = await roleManager.CreateAsync(new IdentityRole
                {
                    Name = request.roleName,
                    NormalizedName = request.roleName.ToUpper(),
                });

                if (result.Succeeded)
                    logger.LogInformation($"The role with name : {request.roleName} is added successfully");
                else
                    logger.LogError($"Can not add the role with name : {request.roleName} because:" +
                        $"{string.Join(',', result.Errors.Select(x => x.Description))}");

                return result.Succeeded
                    ? new ApiResponseDto<AddNewRoleResponseDto>
                    {
                        Result = new AddNewRoleResponseDto { IsAdded = true },
                        Message = string.Empty,
                        IsSucceed = true
                    }
                    : new ApiResponseDto<AddNewRoleResponseDto>
                    {
                        Result = new AddNewRoleResponseDto { IsAdded = false },
                        Message = string.Join(',', result.Errors.Select(x => x.Description)),
                        IsSucceed = false
                    };
            }
            catch(Exception ex)
            {
                logger.LogError($"Can not save the role with name : {request.roleName} because this " +
                    $"exception : {ex.Message}");

                return new ApiResponseDto<AddNewRoleResponseDto>
                {
                    Result = new AddNewRoleResponseDto { IsAdded = false },
                    Message = "An error occur while adding the role",
                    IsSucceed = false
                };
            }
        }
    }
}
