using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Features.Groups;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Models;
using Xunit;

namespace StudentsFirst.Api.Monolithic.Tests.Features.Groups
{
    public class Groups_FindOne_Tests : IDisposable
    {
        public Groups_FindOne_Tests()
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
        public async Task Handle_ByTeacher_ReturnsMatch()
        {
            const string USER_NAME = "User";
            const string USER_ID = "6597a123-91a2-47a5-b30d-43da56456803";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "9bfbc6cd-a9a8-437b-8a4c-42047ca2e7de";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            GroupsService providedGroupsService = new GroupsService(providedContext);

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER),
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(GROUP_ID);
            FindOne.Handler handler = new FindOne.Handler(providedGroupsService, _mapperProvider.Mapper, userAccessorService);
        
            Group expectedEntity = await context.Groups.SingleAsync(g => g.Id == GROUP_ID);

            GroupResponse expectedResponse = _mapperProvider.Mapper.Map<GroupResponse>(expectedEntity);
            GroupResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task Handle_ByStudentOwn_ReturnsMatch()
        {
            const string USER_NAME = "User";
            const string USER_ID = "8698dd7a-1d7a-458e-8b11-1b73be2fb21a";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "a63a31cf-27a5-4344-94c8-a56be9bce7ed";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            GroupsService providedGroupsService = new GroupsService(providedContext);

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(GROUP_ID);
            FindOne.Handler handler = new FindOne.Handler(providedGroupsService, _mapperProvider.Mapper, userAccessorService);
        
            Group expectedEntity = await context.Groups.SingleAsync(g => g.Id == GROUP_ID);

            GroupResponse expectedResponse = _mapperProvider.Mapper.Map<GroupResponse>(expectedEntity);
            GroupResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task Handle_Inexistent_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "3dc5fa16-907b-4473-aabe-dc061fb4afee";

            const string INEXISTENT_GROUP_ID = "00000000-0000-0000-0000-000000000000";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            GroupsService providedGroupsService = new GroupsService(providedContext);

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(INEXISTENT_GROUP_ID);
            FindOne.Handler handler = new FindOne.Handler(providedGroupsService, _mapperProvider.Mapper, userAccessorService);
        
            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_ByStudentNotOwn_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "089440b5-db3a-4795-9a93-bd3914672cd8";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "1282ae1f-8180-49de-9cf5-0487866c04d3";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            GroupsService providedGroupsService = new GroupsService(providedContext);

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(GROUP_ID);
            FindOne.Handler handler = new FindOne.Handler(providedGroupsService, _mapperProvider.Mapper, userAccessorService);
        
            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }
    }
}
