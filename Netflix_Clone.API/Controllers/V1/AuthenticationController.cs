
using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class AuthenticationController(
        ILogger<AuthenticationController> logger,
        ISender sender,
        IOptions<UserRolesOptions> userRolesOptions)
        : BaseController<AuthenticationController>(logger)
    {
        private readonly ISender sender = sender;
        private readonly IOptions<UserRolesOptions> userRolesOptions = userRolesOptions;

        [HttpPost]
        [Route("POST/Register")]
        public async Task<ActionResult<ApiResponseDto<RegistrationResponseDto>>> Register(
            [FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (ModelState.IsValid)
            {
                logger.LogTrace($"Try to register the user with email : {registrationRequestDto.Email}");

                ApplicationDbContext applicationDbContext = new ApplicationDbContext();

                var response = await sender.Send(registrationRequestDto.Adapt<RegisterUserCommand>());

                return response.IsSucceed ? Ok(response) : BadRequest(response);
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
            logger.LogTrace($"Try to add the user role : {RoleName}");

            var command = new AddNewRoleCommand(RoleName);
            var response = await sender.Send(command);

            if (response.IsSucceed && response.Result.IsAdded)
            {
                return Created("", response.Result);
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
                logger.LogTrace($"Try to login the user with email : {loginRequestDto.Email}");

                var response = await sender.Send(new UserLoginCommand(loginRequestDto, HttpContext));

                return response.IsSucceed
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
        [Route("POST/AssignUserToRole")]
        [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
        public async Task<ActionResult<ApiResponseDto<AssignUserToRoleResponseDto>>> AssignUserToRole([FromBody] AssignUserToRoleRequestDto assignUserToRoleRequestDto)
        {
            if (ModelState.IsValid)
            {
                logger.LogTrace($"Try to assign the user with id : {assignUserToRoleRequestDto.UserId}" +
                    $"to the role with name : {assignUserToRoleRequestDto.RoleName}");

                var response = await sender.Send(assignUserToRoleRequestDto.Adapt<AssignUserToRoleCommand>());

                if (response.IsSucceed && response.Result.IsAssigned)
                {
                    return Ok(response);
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
