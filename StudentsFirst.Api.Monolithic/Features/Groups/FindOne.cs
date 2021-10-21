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
            public Handler(StudentsFirstContext dbContext, IMapper mapper, IUserAccessorService userAccessorService)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _userAccessorService = userAccessorService;
            }

            private readonly StudentsFirstContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessorService _userAccessorService;

            public async Task<GroupResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                IQueryable<Group> groups = _dbContext.Groups;

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

                return _mapper.Map<GroupResponse>(foundGroup);
            }
        }
    }
}
