
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Commands;

namespace Netflix_Clone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IMediator mediator;

        public AuthenticationController(ILogger<AuthenticationController> logger,
            IMediator mediator)
            : base(logger)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("POST/Register")]
        public async Task<ActionResult<RegistrationResponseDto>> Register([FromBody]RegistrationRequestDto registrationRequestDto)
        {
            if (ModelState.IsValid)
            {
                return default;
            }
            else
            {
                return BadRequest(new RegistrationResponseDto { IsRegistered = false, Message = "Invalid Model State" });
            }
        }

        [HttpPost]
        [Route("POST/AddNewUserRole")]
        public async Task<ActionResult<bool>> AddNewUserRole([FromBody] string RoleName)
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

    }
}
