using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
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
                
                if (enforceRestrictedSet)
                {
                    users = users
                        .Where(otherUser => otherUser.Id == user.Id || otherUser.Role == RoleConstants.TEACHER || (
                            from otherUserGroupMembership in _dbContext.UserGroupMemberships
                            where otherUserGroupMembership.UserId == otherUser.Id
                            from sharedUserGroupMembership in _dbContext.UserGroupMemberships
                            where
                                sharedUserGroupMembership.GroupId == otherUserGroupMembership.GroupId
                                && sharedUserGroupMembership.UserId == user.Id
                            select sharedUserGroupMembership
                        ).Any());
                }

                User foundUser = await users.SingleOrDefaultAsync(u => u.Id == request.UserId)
                    ?? throw new NotFoundRestException(nameof(User));

                return _mapper.Map<UserResponse>(foundUser);
            }
        }
    }
}
