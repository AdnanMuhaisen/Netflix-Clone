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

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class UserLoginCommandHandler(UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator) 
        : IRequestHandler<UserLoginCommand, ApiResponseDto>
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator = jwtTokenGenerator;

        public async Task<ApiResponseDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.loginRequestDto.Email);
            if (user  is null || !await userManager.CheckPasswordAsync(user, request.loginRequestDto.Password))
            {
                return new ApiResponseDto
                {
                    Result = new LoginResponseDto
                    {
                        UserDto = default!,
                    },
                    Message = "Invalid Username Or Password"
                };
            }

            //generate token
            var userRoles = await userManager.GetRolesAsync(user);
            string token = jwtTokenGenerator.GenerateToken(user, userRoles);

            return new ApiResponseDto
            {
                Result = new LoginResponseDto
                {
                    UserDto = user.Adapt<ApplicationUserDto>(),
                    Token = token,
                },
                Message = "The user logged in successfully"
            };
        }

        private async Task IdentitySignInAsync(ApplicationUser applicationUser , string Token,HttpContext httpContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(Token);

            var identity = new ClaimsIdentity();
            identity.AddClaims(
            [
                new Claim(JwtRegisteredClaimNames.Sub,
                jwtToken.Claims.First(x=>x.Type == JwtRegisteredClaimNames.Sub).Value),

                new Claim(JwtRegisteredClaimNames.Email,
                jwtToken.Claims.First(x=>x.Type == JwtRegisteredClaimNames.Email).Value),

                new Claim(JwtRegisteredClaimNames.Name,
                jwtToken.Claims.First(x=>x.Type == JwtRegisteredClaimNames.Name).Value),
            ]);


            foreach (var claim in jwtToken.Claims.Where(x=>x.Type == "role"))
            {
                identity.AddClaim(new Claim("role", claim.Value));
            }
            identity.AddClaim(new Claim(ClaimTypes.Name, applicationUser.UserName!));

            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(IdentityConstants.ExternalScheme, principal);
        }
    }
}
