using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Users
{
    public class FindAll
    {
        public record Request(
            string? NameIncludes,
            string? Role,
            int Skip,
            int Take
        ) : FindAllUsersRequest(NameIncludes, Role, Skip, Take), IRequest<UsersResponse>;

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(r => r.Role)
                    .Must(r => r == null || (new List<string>() { RoleConstants.STUDENT, RoleConstants.TEACHER }).Contains(r));
            }
        }

        public class Handler : IRequestHandler<Request, UsersResponse>
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

            public async Task<UsersResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                IQueryable<User> users = _dbContext.Users;

                bool filtering = false;
                int skipping = request.Skip;
                int taking = request.Take;

                bool enforceRestrictedSet = user.IsStudent;

                if (enforceRestrictedSet)
                {
                    users = users
                        .Where(u => u.Id == user.Id || u.Role == RoleConstants.TEACHER)
                        .Concat(
                            from ownUserGroupMembership in _dbContext.UserGroupMemberships
                            where ownUserGroupMembership.UserId == user.Id
                            join @group in _dbContext.Groups on ownUserGroupMembership.GroupId equals @group.Id
                            from sharedUserGroupMembership in _dbContext.UserGroupMemberships
                            where sharedUserGroupMembership.GroupId == @group.Id
                            join user2 in _dbContext.Users on sharedUserGroupMembership.UserId equals user2.Id
                            where !(user2.Id == user.Id || user2.Role == RoleConstants.TEACHER)
                            select user2
                        );
                }

                if (!string.IsNullOrEmpty(request.NameIncludes))
                {
                    users = users.Where(g => g.Name.Contains(request.NameIncludes));
                    filtering = true;
                }

                if (!string.IsNullOrEmpty(request.Role))
                {
                    users = users.Where(g => g.Role == request.Role);
                    filtering = true;
                }

                users = users.OrderBy(u => u.Name).Skip(skipping).Take(taking);

                IList<UserResponse> response = _mapper.Map<IList<UserResponse>>(await users.ToListAsync());

                return new UsersResponse(response, filtering, skipping, taking);
            }
        }
    }
}
