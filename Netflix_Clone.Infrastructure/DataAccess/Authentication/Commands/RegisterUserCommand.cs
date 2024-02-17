using MediatR;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands
{
    public class RegisterUserCommand(RegistrationRequestDto registrationRequestDto)
        : IRequest<ApiResponseDto<RegistrationResponseDto>>
    {
        public readonly RegistrationRequestDto registrationRequestDto = registrationRequestDto;
    }
}
