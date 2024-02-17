using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger,
        UserManager<ApplicationUser> userManager,
        IOptions<UserRolesOptions> options)
        : IRequestHandler<RegisterUserCommand, ApiResponseDto<RegistrationResponseDto>>
    {
        private readonly ILogger<RegisterUserCommandHandler> logger = logger;
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IOptions<UserRolesOptions> options = options;

        public async Task<ApiResponseDto<RegistrationResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The registration command handler is start to execute");

            if(await userManager.FindByEmailAsync(request.registrationRequestDto.Email) is not null)
            {
                logger.LogError("The user with email : {email} is already exist", request.registrationRequestDto.Email);

                return new ApiResponseDto<RegistrationResponseDto>
                {
                    Result = new RegistrationResponseDto
                    {
                        IsRegistered = false,
                    },
                    Message = $"The user with the email : {request.registrationRequestDto.Email} is already exist",
                    IsSucceed = true
                };
            }

            // name => username in the table
            if(await userManager.FindByNameAsync($"{request.registrationRequestDto.FirstName}{request.registrationRequestDto.FirstName}") is not null)
            {
                logger.LogError("The user with Name : {name} is already exist",
                    $"{request.registrationRequestDto.FirstName}{request.registrationRequestDto.FirstName}");

                return new ApiResponseDto<RegistrationResponseDto>
                {
                    Result = new RegistrationResponseDto
                    {
                        IsRegistered = false,
                    },
                    Message = $"The user with the UserName : {request.registrationRequestDto.FirstName}{request.registrationRequestDto.FirstName} is already exist",
                    IsSucceed = true
                };
            }

            // add the user :
            var userCreationResult = await userManager.CreateAsync(new ApplicationUser
            {
                FirstName = request.registrationRequestDto.FirstName,
                LastName = request.registrationRequestDto.LastName,
                Email = request.registrationRequestDto.Email,
                UserName = $"{request.registrationRequestDto.FirstName}{request.registrationRequestDto.LastName}",
                PhoneNumber = request.registrationRequestDto.PhoneNumber,
                NormalizedUserName = $"{request.registrationRequestDto.FirstName}{request.registrationRequestDto.LastName}".ToUpper(),
                NormalizedEmail = request.registrationRequestDto.Email.ToUpper()
            }, request.registrationRequestDto.Password);

            if (!userCreationResult.Succeeded)
            {
                logger.LogError("An error occur while saving the user with email : {email}",
                    request.registrationRequestDto.Email);

                return new ApiResponseDto<RegistrationResponseDto>
                {
                    Result = new RegistrationResponseDto
                    {
                        IsRegistered = false,
                    },
                    Message = string.Join(',', userCreationResult.Errors.Select(x => x.Description)),
                    IsSucceed = true
                };
            }

            logger.LogTrace("The user is added successfully");

            var registeredUser = await userManager.FindByEmailAsync(request.registrationRequestDto.Email);

            if(registeredUser is null)
            {
                logger.LogError("An Error occur while finding the user with email : {email} ", request.registrationRequestDto.Email);

                return new ApiResponseDto<RegistrationResponseDto>
                {
                    Result = new RegistrationResponseDto
                    {
                        IsRegistered = true,
                    },
                    Message = "Can not find the user after it is added successfully",
                    IsSucceed = true
                };
            }

            var addToRoleResult = await userManager.AddToRoleAsync(registeredUser, options.Value.ApplicationUser);

            if(!addToRoleResult.Succeeded)
            {
                logger.LogError("An Error occur while adding the user with id : {id} to the USER role", registeredUser.Id);

                return new ApiResponseDto<RegistrationResponseDto>
                {
                    Result = new RegistrationResponseDto
                    {
                        IsRegistered = true,
                    },
                    Message = $"The user is registered successfully ,but can not add this user to role due to " +
                    $"this errors : {string.Join(',', addToRoleResult.Errors.Select(x => x.Description))}",
                    IsSucceed = true
                };
            }

            logger.LogTrace("The user with id : {id} is added to the USER Role Successfully", registeredUser.Id);

            return new ApiResponseDto<RegistrationResponseDto>
            {
                Result = new RegistrationResponseDto
                {
                    UserId = registeredUser.Id,
                    UserName = registeredUser.UserName!,
                    Email = registeredUser.Email!,
                    IsRegistered = true,
                },
                Message = "Registered successfully",
                IsSucceed = true
            };
        }
    }
}
