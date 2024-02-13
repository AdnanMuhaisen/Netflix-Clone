using MediatR;
using Microsoft.AspNetCore.Http;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class UserLoginCommand(LoginRequestDto loginRequestDto,HttpContext httpContext) : IRequest<LoginResponseDto>
    {
        public readonly LoginRequestDto loginRequestDto = loginRequestDto;
        public readonly HttpContext httpContext = httpContext;
    }
}
