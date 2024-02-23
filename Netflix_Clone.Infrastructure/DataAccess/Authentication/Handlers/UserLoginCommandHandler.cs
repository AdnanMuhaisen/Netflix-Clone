using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Handlers
{
    public class UserLoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<UserLoginCommandHandler> logger) 
        : IRequestHandler<UserLoginCommand, ApiResponseDto<LoginResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator = jwtTokenGenerator;
        private readonly ILogger<UserLoginCommandHandler> logger = logger;

        public async Task<ApiResponseDto<LoginResponseDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.loginRequestDto.Email);

            if (user  is null || !await userManager.CheckPasswordAsync(user, request.loginRequestDto.Password))
            {
                logger.LogTrace($"Can not login the user with email {request.loginRequestDto.Email}" +
                    $"due to invalid email or password");

                return new ApiResponseDto<LoginResponseDto>
                {
                    Result = new LoginResponseDto
                    {
                        UserDto = default!,
                    },
                    Message = "Invalid Username Or Password",
                    IsSucceed = false
                };
            }

            //generate token
            var userRoles = await userManager.GetRolesAsync(user);
            string token = jwtTokenGenerator.GenerateToken(user, userRoles);

            logger.LogTrace($"The user token is generated for the user with email {request.loginRequestDto.Email}");

            logger.LogInformation($"The user with email {request.loginRequestDto.Email} is logged in");

            return new ApiResponseDto<LoginResponseDto>
            {
                Result = new LoginResponseDto
                {
                    UserDto = user.Adapt<ApplicationUserDto>(),
                    Token = token,
                },
                Message = "The user logged in successfully",
                IsSucceed = true
            };
        }
    }
}
