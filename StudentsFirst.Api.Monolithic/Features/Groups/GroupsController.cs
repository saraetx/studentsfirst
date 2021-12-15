using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Groups;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    [ApiController, Route("groups")]
    [Authorize(Roles = $"{RoleConstants.STUDENT},{RoleConstants.TEACHER}"), RequiredScope(ScopeConstants.LOGIN)]
    public class GroupsController : ControllerBase
    {
        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private readonly IMediator _mediator;

        [HttpGet, Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupsResponse))]
        public async Task<GroupsResponse> FindAll(
            [FromQuery] string? nameIncludes = null,
            [FromQuery] bool ownOnly = false,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 100
        )
        {
            return await _mediator.Send(new FindAll.Request(nameIncludes, ownOnly, skip, take));
        }

        [HttpGet, Route("{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<GroupResponse> FindOne([FromRoute] string groupId)
        {
            return await _mediator.Send(new FindOne.Request(groupId));
        }

        [HttpPost, Route("")]
        [Authorize(Roles = RoleConstants.TEACHER)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GroupResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupResponse>> Create([FromBody] Create.Body body, [FromQuery] bool addSelf = true)
        {
            GroupResponse response = await _mediator.Send(new Create.Request(body, addSelf));
            return CreatedAtAction(nameof(FindOne), new { groupId = response.Id }, response);
        }

        [HttpGet, Route("{groupId}/members")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupMembersResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<GroupMembersResponse> ListMembers(
            [FromRoute] string groupId,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 100
        )
        {
            return await _mediator.Send(new ListMembers.Request(groupId, skip, take));
        }
        
        [HttpPut, Route("{groupId}/members/{userId}")]
        [Authorize(Roles = RoleConstants.TEACHER)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddMember([FromRoute] string groupId, [FromRoute] string userId)
        {
            bool created = await _mediator.Send(new AddMember.Request(groupId, userId));
            return created ? CreatedAtAction(nameof(ListMembers), new { groupId = groupId }, null) : NoContent();
        }

        [HttpDelete, Route("{groupId}/members/{userId}")]
        [Authorize(Roles = RoleConstants.TEACHER)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveMember([FromRoute] string groupId, [FromRoute] string userId)
        {
            await _mediator.Send(new RemoveMember.Request(groupId, userId));
            return NoContent();
        }
    }
}
