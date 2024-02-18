
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;
using Netflix_Clone.Shared.Enums;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly ISender mediator;
        private readonly IOptions<UserRolesOptions> userRolesOptions;

        public AuthenticationController(ILogger<AuthenticationController> logger,
            IMediator mediator,
            IOptions<UserRolesOptions> userRolesOptions)
            : base(logger)
        {
            this.mediator = mediator;
            this.userRolesOptions = userRolesOptions;
        }

        [HttpPost]
        [Route("POST/Register")]
        public async Task<ActionResult<ApiResponseDto<RegistrationResponseDto>>> Register([FromBody]RegistrationRequestDto registrationRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(registrationRequestDto.Adapt<RegisterUserCommand>());
                return (response.IsSucceed) ? Ok(response) : BadRequest(response);
            }
            else
            {
                return BadRequest(new ApiResponseDto<RegistrationResponseDto>
                {
                    Result = new RegistrationResponseDto { IsRegistered = false },
                    Message = "Invalid Model State",
                    IsSucceed = false
                });
            }
        }

        [HttpPost]
        [Route("POST/AddNewUserRole/{RoleName}")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<AddNewRoleResponseDto>>> AddNewUserRole([FromRoute] string RoleName)
        {
            var command = new AddNewRoleCommand(RoleName);
            var response = await mediator.Send(command);

            if (response.IsSucceed)
            {
                return response.Result.IsAdded
                    ? Created("", response.Result)
                    : BadRequest(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("POST/Login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(new UserLoginCommand(loginRequestDto, HttpContext));
                return (response.IsSucceed)
                ? Ok(response)
                : BadRequest(response);
            }
            else
            {
                return BadRequest(new LoginResponseDto
                {
                    UserDto = default!
                });
            }
        }

        [HttpPost]
        [Route("POST/AssignRoleToUser")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<AssignUserToRoleResponseDto>>> AssignUserToRole([FromBody] AssignUserToRoleRequestDto assignUserToRoleRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(assignUserToRoleRequestDto.Adapt<AssignUserToRoleCommand>());

                if (response.IsSucceed)
                {
                    return (response.Result.IsAssigned)
                    ? Ok(response)
                    : BadRequest(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return new ApiResponseDto<AssignUserToRoleResponseDto>
                {
                    Result = null!,
                    Message = "Invalid Model State",
                    IsSucceed = true
                };
            }
        }
    }
}
