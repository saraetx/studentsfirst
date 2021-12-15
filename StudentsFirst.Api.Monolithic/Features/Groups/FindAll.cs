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
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class FindAll
    {
        public record Request(
            string? NameIncludes,
            bool OwnOnly,
            int Skip,
            int Take
        ) : FindAllGroupsRequest(NameIncludes, OwnOnly, Skip, Take), IRequest<GroupsResponse>;

        public class Handler : IRequestHandler<Request, GroupsResponse>
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

            public async Task<GroupsResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                IQueryable<Group> groups = _dbContext.Groups;

                bool filtering = false;
                int skipping = request.Skip;
                int taking = request.Take;

                bool enforceOwnOnly = user.IsStudent;

                if (request.OwnOnly || enforceOwnOnly)
                {
                    groups =
                        from @group in groups
                        join userGroupMembership in _dbContext.UserGroupMemberships on @group.Id equals userGroupMembership.GroupId
                        where userGroupMembership.UserId == user.Id
                        select @group;

                    if (request.OwnOnly) { filtering = true; }
                }

                if (!string.IsNullOrEmpty(request.NameIncludes))
                {
                    groups = groups.Where(g => g.Name.ToLower().Contains(request.NameIncludes.ToLower()));
                    filtering = true;
                }

                int total = await groups.CountAsync();
                groups = groups.OrderBy(g => g.Name).Skip(skipping).Take(taking);

                IList<GroupResponse> response = _mapper.Map<IList<GroupResponse>>(await groups.ToListAsync());

                return new GroupsResponse(response, filtering, total, skipping, taking);
            }
        }
    }
}
