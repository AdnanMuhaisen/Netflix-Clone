using MediatR;
using Microsoft.AspNetCore.Http;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands
{
    public class UserLoginCommand(LoginRequestDto loginRequestDto, HttpContext httpContext) 
        : IRequest<ApiResponseDto<LoginResponseDto>>
    {
        public readonly LoginRequestDto loginRequestDto = loginRequestDto;
        public readonly HttpContext httpContext = httpContext;
    }
}
