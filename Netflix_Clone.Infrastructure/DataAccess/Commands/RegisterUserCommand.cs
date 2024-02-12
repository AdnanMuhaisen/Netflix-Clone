using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Commands
{
    public class RegisterUserCommand(RegistrationRequestDto registrationRequestDto) : IRequest<RegistrationResponseDto>
    {
        public readonly RegistrationRequestDto registrationRequestDto = registrationRequestDto;
    }
}
