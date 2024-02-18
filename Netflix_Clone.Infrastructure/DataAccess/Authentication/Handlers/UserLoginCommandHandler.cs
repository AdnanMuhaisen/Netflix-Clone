using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Handlers
{
    public class UserLoginCommandHandler(UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator) 
        : IRequestHandler<UserLoginCommand, ApiResponseDto<LoginResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator = jwtTokenGenerator;

        public async Task<ApiResponseDto<LoginResponseDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.loginRequestDto.Email);
            if (user  is null || !await userManager.CheckPasswordAsync(user, request.loginRequestDto.Password))
            {
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
