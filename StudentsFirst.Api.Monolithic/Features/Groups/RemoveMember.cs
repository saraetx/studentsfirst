using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class RemoveMember
    {
        public record Request(
            string GroupId,
            string UserId
        ) : RemoveMemberFromGroupRequest(GroupId, UserId), IRequest;

        public class Handler : IRequestHandler<Request>
        {
            public Handler(StudentsFirstContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            private readonly StudentsFirstContext _dbContext;
            private readonly IMapper _mapper;

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken = default)
            {
                Group group = await _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken)
                    ?? throw new NotFoundRestException(nameof(Group));

                User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                    ?? throw new NotFoundRestException(nameof(User));

                UserGroupMembership? userGroupMembership = await _dbContext.UserGroupMemberships.SingleOrDefaultAsync(
                    ugm => ugm.UserId == user.Id && ugm.GroupId == group.Id,
                    cancellationToken
                ) ?? throw new NotFoundRestException(nameof(UserGroupMembership));

                _dbContext.UserGroupMemberships.Remove(userGroupMembership);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
