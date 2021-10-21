using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class AddMember
    {
        public record Request(
            string GroupId,
            string UserId
        ) : AddMemberToGroupRequest(GroupId, UserId), IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            public Handler(StudentsFirstContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            private readonly StudentsFirstContext _dbContext;
            private readonly IMapper _mapper;

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken = default)
            {
                Group group = await _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken)
                    ?? throw new NotFoundRestException(nameof(Group));

                User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                    ?? throw new NotFoundRestException(nameof(User));

                bool alreadyExists = await _dbContext.UserGroupMemberships.AnyAsync(
                    ugm => ugm.UserId == user.Id && ugm.GroupId == group.Id,
                    cancellationToken
                );

                if (!alreadyExists)
                {
                    UserGroupMembership userGroupMembership = new UserGroupMembership(user.Id, group.Id);

                    _dbContext.UserGroupMemberships.Add(userGroupMembership);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return true;
                }
                else { return false; }
            }
        }
    }
}
