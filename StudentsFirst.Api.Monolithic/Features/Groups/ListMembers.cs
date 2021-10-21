using System.Collections.Generic;
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
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class ListMembers
    {
        public record Request(
            string GroupId,
            int Skip,
            int Take
        ) : ListMembersInGroupRequest(GroupId, Skip, Take), IRequest<GroupMembersResponse>;

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator() { }
        }

        public class Handler : IRequestHandler<Request, GroupMembersResponse>
        {
            public Handler(StudentsFirstContext dbContext, IMapper mapper, IUserAccessorService userAccessorService)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _userAccessorService = userAccessorService;
            }

            private readonly StudentsFirstContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessorService _userAccessorService;

            public async Task<GroupMembersResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                IQueryable<UserGroupMembership> userGroupMemberships = _dbContext.UserGroupMemberships;

                IQueryable<Group> groups = _dbContext.Groups;

                int skipping = request.Skip;
                int taking = request.Take;

                bool enforceOwnOnly = user.Role == RoleConstants.STUDENT;

                if (enforceOwnOnly)
                {
                    groups =
                        from @group in groups
                        join userGroupMembership in _dbContext.UserGroupMemberships on @group.Id equals userGroupMembership.GroupId
                        where userGroupMembership.UserId == user.Id
                        select @group;
                }

                Group foundGroup = await groups.SingleOrDefaultAsync(g => g.Id == request.GroupId)
                    ?? throw new NotFoundRestException(nameof(Group));

                IQueryable<User> members =
                    from userGroupMembership in userGroupMemberships
                    where userGroupMembership.GroupId == foundGroup.Id
                    join member in _dbContext.Users on userGroupMembership.UserId equals member.Id
                    select member;
                
                members = members.OrderBy(m => m.Name).Skip(skipping).Take(taking);

                GroupResponse groupResponse = _mapper.Map<GroupResponse>(foundGroup);
                IList<UserResponse> response = _mapper.Map<IList<UserResponse>>(await members.ToListAsync());

                return new GroupMembersResponse(groupResponse, response, skipping, taking);
            }
        }
    }
}
