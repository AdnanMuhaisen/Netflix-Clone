
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Commands;

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
                return BadRequest(new RegistrationResponseDto { IsRegistered = false, Message = "Invalid Model State" });
            }
        }

        [HttpPost]
        [Route("POST/AddNewUserRole/{RoleName}")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME,Roles = ADMIN_ROLE)]
        public async Task<ActionResult<bool>> AddNewUserRole([FromRoute] string RoleName)
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                return BadRequest(false);
            }

            var command = new AddNewRoleCommand(RoleName);
            var result = await mediator.Send(command);

            return (result.IsAdded)
                ? Created("", result)
                : BadRequest(result);
        }

        [HttpPost]
        [Route("POST/Login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                var command = new UserLoginCommand(loginRequestDto, HttpContext);
                var result = await mediator.Send(command);
                return (result.UserDto is null) ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest(new LoginResponseDto
                {
                    UserDto = default!,
                    Message = "Invalid Model State"
                });
            }
        }

        [HttpPost]
        [Route("POST/AssignRoleToUser")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME,Roles = ADMIN_ROLE)]
        public async Task<ActionResult<AssignUserToRoleResponseDto>> AssignUserToRole([FromBody] AssignUserToRoleRequestDto assignUserToRoleRequestDto)
        {
            if(ModelState.IsValid)
            {
                var command = new AssignUserToRoleCommand(assignUserToRoleRequestDto);
                var result = await mediator.Send(command);
                return (result.IsAssigned) ? Ok(result) : BadRequest(result);
            }
            else
            {
                return new AssignUserToRoleResponseDto
                {
                    IsAssigned = false,
                    Message = "Invalid Model State"
                };
            }
        }





    }
}
