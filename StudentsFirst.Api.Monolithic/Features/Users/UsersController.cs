using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Users;

namespace StudentsFirst.Api.Monolithic.Features.Users
{
    [ApiController, Route("users")]
    [Authorize(Roles = $"{RoleConstants.STUDENT},{RoleConstants.TEACHER}"), RequiredScope(ScopeConstants.LOGIN)]
    public class UsersController : ControllerBase
    {
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private readonly IMediator _mediator;

        [HttpGet, Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsersResponse))]
        public async Task<UsersResponse> FindAll(
            [FromQuery] string? nameIncludes = null,
            [FromQuery] string? role = null,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 100
        )
        {
            return await _mediator.Send(new FindAll.Request(nameIncludes, role, skip, take));
        }

        [HttpGet, Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<UserResponse> FindOne([FromRoute] string userId)
        {
            return await _mediator.Send(new FindOne.Request(userId));
        }
    }
}
