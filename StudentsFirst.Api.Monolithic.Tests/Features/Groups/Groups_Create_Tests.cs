using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Features.Groups;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;
using Xunit;

namespace StudentsFirst.Api.Monolithic.Tests.Features.Groups
{
    public class Groups_Create_Tests : IDisposable
    {
        public Groups_Create_Tests()
        {
            _mapperProvider = new MapperProvider();
            _dbContextProvider = new InMemoryApplicationDbContextProvider();
        }

        public void Dispose()
        {
            _dbContextProvider.Dispose();
        }

        private readonly IMapperProvider _mapperProvider;
        private readonly IDbContextProvider<StudentsFirstContext> _dbContextProvider;

        [Fact]
        public async Task Handle_WithoutAddSelf_CreatesEmptyGroup()
        {
            const string GROUP_NAME = "Group";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            IUserAccessorService userAccessorService = new UserAccessorService(null);

            Create.Request request = new Create.Request(new Create.Body(GROUP_NAME), AddSelf: false);
            Create.Handler handler = new Create.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            GroupResponse actualResponse = await handler.Handle(request);

            Group? createdGroup = await context.Groups.SingleOrDefaultAsync();

            Assert.NotNull(createdGroup);
            Assert.Equal(GROUP_NAME, createdGroup.Name);

            IList<UserGroupMembership> groupMemberships = await context.UserGroupMemberships.ToListAsync();

            Assert.Empty(groupMemberships);

            GroupResponse expectedResponse = _mapperProvider.Mapper.Map<GroupResponse>(createdGroup);

            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task Handle_AddSelf_CreatesGroupWithSelf()
        {
            const string USER_NAME = "User";
            const string USER_ID = "a513eed3-be74-47cc-9433-9ae39af6d0cb";

            const string GROUP_NAME = "Group";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER),
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            Create.Request request = new Create.Request(new Create.Body(GROUP_NAME), AddSelf: true);
            Create.Handler handler = new Create.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            GroupResponse actualResponse = await handler.Handle(request);

            Group? createdGroup = await context.Groups.SingleOrDefaultAsync();

            Assert.NotNull(createdGroup);

            UserGroupMembership? createdGroupMembership = await context.UserGroupMemberships
                .SingleOrDefaultAsync();

            Assert.NotNull(createdGroupMembership);
            
            Assert.Equal(user.Id, createdGroupMembership.UserId);
            Assert.Equal(createdGroup.Id, createdGroupMembership.GroupId);
        }
    }
}
