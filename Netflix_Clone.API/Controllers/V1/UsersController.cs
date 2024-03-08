
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.ELS.Users.Queries;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = BEARER_AUTHENTICATION_SCHEME, Roles = ADMIN_ROLE)]
    public class UsersController(ILogger<UsersController> logger,ISender sender) 
        : BaseController<UsersController>(logger)
    {
        private readonly ISender sender = sender;

        [HttpGet]
        [Route("GET/Search")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<ApiResponseDto<ELSSearchResponse<UserDocument>>>> Search([FromQuery] string searchQuery)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(searchQuery);

            var searchResponse = await sender.Send(new SearchByUserNameQuery(searchQuery));

            return new ApiResponseDto<ELSSearchResponse<UserDocument>>
            {
                IsSucceed = true,
                Result = searchResponse
            };
        }
    }
}
