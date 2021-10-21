using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Features.Groups
{
    public class Create
    {
        public record Request(Body Body, bool AddSelf) : CreateGroupRequest(Body.Name, AddSelf), IRequest<GroupResponse>;
        public record Body(string Name);

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(r => r.Name).NotEmpty();
            }
        }

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
                Group group = new Group(id: Guid.NewGuid().ToString(), request.Name);
                _dbContext.Groups.Add(group);

                if (request.AddSelf)
                {
                    UserGroupMembership userGroupMembership = new UserGroupMembership(_userAccessorService.User!.Id, group.Id);
                    _dbContext.UserGroupMemberships.Add(userGroupMembership);
                }
                
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<GroupResponse>(group);
            }
        }
    }
}
