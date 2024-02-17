
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IMediator mediator;
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
        public async Task<ActionResult<RegistrationResponseDto>> Register([FromBody]RegistrationRequestDto registrationRequestDto)
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterUserCommand(registrationRequestDto);
                var result = await mediator.Send(command);
                return (result is null) ? BadRequest(result) : Created("", result);
            }
            else
            {
                return null!;// BadRequest(new RegistrationResponseDto { IsRegistered = false, Message = "Invalid Model State" });
            }
        }

        [HttpPost]
        [Route("POST/AddNewUserRole/{RoleName}")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME,Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> AddNewUserRole([FromRoute] string RoleName)
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                return BadRequest(false);
            }

            var command = new AddNewRoleCommand(RoleName);
            var response = await mediator.Send(command);

            return (((AddNewRoleResponseDto)(response.Result)).IsAdded)
                ? Created("", response)
                : BadRequest(response);
        }

        [HttpPost]
        [Route("POST/Login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                var command = new UserLoginCommand(loginRequestDto, HttpContext);
                var result = await mediator.Send(command);
                return null!;//(result.UserDto is null) ? BadRequest(result) : Ok(result);
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
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME,Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto>> AssignUserToRole([FromBody] AssignUserToRoleRequestDto assignUserToRoleRequestDto)
        {
            if(ModelState.IsValid)
            {
                var command = new AssignUserToRoleCommand(assignUserToRoleRequestDto);
                var response = await mediator.Send(command);
                return (((AssignUserToRoleResponseDto)(response.Result)).IsAssigned)
                    ? Ok(response)
                    : BadRequest(response);
            }
            else
            {
                return new ApiResponseDto
                {
                    Result = null!,
                    Message = "Invalid Model State"
                };
            }
        }
    }
}
