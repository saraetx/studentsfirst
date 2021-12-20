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
using StudentsFirst.Common.Services;

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
            public Handler(IGroupsService groupsService, IMapper mapper, IUserAccessorService userAccessorService)
            {
                _groupsService = groupsService;
                _mapper = mapper;
                _userAccessorService = userAccessorService;
            }

            private readonly IGroupsService _groupsService;
            private readonly IMapper _mapper;
            private readonly IUserAccessorService _userAccessorService;

            public async Task<GroupsResponse> Handle(Request request, CancellationToken cancellationToken = default)
            {
                User user = _userAccessorService.User!;

                bool enforceOwnOnly = user.IsStudent;
                bool filterForOwnOnly = request.OwnOnly;
                bool filterByNameIncludes = !string.IsNullOrEmpty(request.NameIncludes);

                var (groups, total) = await _groupsService.FindAllAsync(
                    withUserId: filterForOwnOnly || enforceOwnOnly ? user.Id : null,
                    nameIncludes: filterByNameIncludes ? request.NameIncludes : null,
                    request.Skip,
                    request.Take
                );

                IList<GroupResponse> response = _mapper.Map<IList<GroupResponse>>(groups);
                bool filtering = filterForOwnOnly || filterByNameIncludes;
                int skipping = request.Skip;
                int taking = request.Take;

                return new GroupsResponse(response, filtering, total, skipping, taking);
            }
        }
    }
}
