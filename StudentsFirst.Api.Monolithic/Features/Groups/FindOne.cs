using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class FindOne
    {
        public record Request(
            string GroupId
        ) : FindOneGroupRequest(GroupId), IRequest<GroupResponse>;

        public class Handler : IRequestHandler<Request, GroupResponse>
        {
            public Handler(GroupsService groupsService, IMapper mapper, IUserAccessorService userAccessorService)
            {
                _groupsService = groupsService;
                _mapper = mapper;
                _userAccessorService = userAccessorService;
            }

            private readonly GroupsService _groupsService;
            private readonly IMapper _mapper;
            private readonly IUserAccessorService _userAccessorService;

            public async Task<GroupResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                bool enforceOwnOnly = user.IsStudent;

                Group group = await _groupsService.FindByGroupIdOrDefaultAsync(
                    request.GroupId,
                    withUserId: enforceOwnOnly ? user.Id : null
                ) ?? throw new NotFoundRestException(nameof(Group));

                return _mapper.Map<GroupResponse>(group);
            }
        }
    }
}
