using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Users
{
    public class FindOne
    {
        public record Request(string Id) : FindOneUserRequest(Id), IRequest<UserResponse>;

        public class Handler : IRequestHandler<Request, UserResponse>
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

            public async Task<UserResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                IQueryable<User> users = _dbContext.Users;

                bool enforceRestrictedSet = user.IsStudent;

                User foundUser = await users.SingleOrDefaultAsync(u => u.Id == request.UserId)
                    ?? throw new NotFoundRestException(nameof(User));
                
                if (enforceRestrictedSet && foundUser.Id != user.Id && !foundUser.IsTeacher)
                {
                    bool isInSameGroup = await (
                        from ownUserGroupMembership in _dbContext.UserGroupMemberships
                        where ownUserGroupMembership.UserId == user.Id
                        join @group in _dbContext.Groups on ownUserGroupMembership.GroupId equals @group.Id
                        from sharedUserGroupMembership in _dbContext.UserGroupMemberships
                        where sharedUserGroupMembership.GroupId == @group.Id && sharedUserGroupMembership.UserId == foundUser.Id
                        select sharedUserGroupMembership
                    ).AnyAsync();

                    if (!isInSameGroup) { throw new NotFoundRestException(nameof(User)); }
                }

                return _mapper.Map<UserResponse>(foundUser);
            }
        }
    }
}
