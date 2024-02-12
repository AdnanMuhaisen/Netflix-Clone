using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegistrationResponseDto>
    {
        private readonly ILogger<RegisterUserCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<RegistrationResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if(await userManager.FindByEmailAsync(request.registrationRequestDto.Email) is not null)
            {
                return new RegistrationResponseDto
                {
                    IsRegistered = false,
                    Message = $"The user with the email : {request.registrationRequestDto.Email} is already exist"
                };
            }

            // name => username in the table
            if(await userManager.FindByNameAsync($"{request.registrationRequestDto.FirstName} {request.registrationRequestDto.FirstName}") is not null)
            {
                return new RegistrationResponseDto
                {
                    IsRegistered = false,
                    Message = $"The user with the UserName : {request.registrationRequestDto.FirstName} {request.registrationRequestDto.FirstName} is already exist"
                };
            }

            // add the user :

            return null;
        }
    }
}
