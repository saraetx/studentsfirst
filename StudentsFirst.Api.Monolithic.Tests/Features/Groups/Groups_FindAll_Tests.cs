using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Groups_FindAll_Tests : IDisposable
    {
        public Groups_FindAll_Tests()
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
        public async Task Handle_ByTeacher_ReturnsAllOrdered()
        {
            const string USER_NAME = "User";
            const string USER_ID = "c3361de6-9630-4076-9b7a-f61df4ee4953";

            const string GROUP_1_NAME = "b. Group 1";
            const string GROUP_1_ID = "b4d5cf6b-e105-4f8d-80b0-b51cbb68bd2c";
            const string GROUP_2_NAME = "a. Group 2";
            const string GROUP_2_ID = "0e88ce0d-5bc2-450d-8bcc-b41f8d5cdfff";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER),
                new Group(GROUP_1_ID, GROUP_1_NAME),
                new Group(GROUP_2_ID, GROUP_2_NAME),
                new UserGroupMembership(USER_ID, GROUP_1_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, OwnOnly: false, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<Group> expectedEntities = await context.Groups.OrderBy(g => g.Name).ToListAsync();

            IList<GroupResponse> expectedGroups = _mapperProvider.Mapper.Map<IList<GroupResponse>>(expectedEntities);
            GroupsResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedGroups, actualResponse.Groups);
            Assert.False(actualResponse.Filtering);
        }

        [Fact]
        public async Task Handle_ByTeacherOwnOnly_ReturnsOwnOnly()
        {
            const string USER_NAME = "User";
            const string USER_ID = "dc7502a6-9ff5-4136-aa42-b0d199e75b17";

            const string GROUP_1_NAME = "Group 1";
            const string GROUP_1_ID = "f5e900c5-3206-40d4-8a39-87416287a9d1";
            const string GROUP_2_NAME = "Group 2";
            const string GROUP_2_ID = "c9ff00ae-f6cb-4a6e-a33e-7a80d7b62a92";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER),
                new Group(GROUP_1_ID, GROUP_1_NAME),
                new Group(GROUP_2_ID, GROUP_2_NAME),
                new UserGroupMembership(USER_ID, GROUP_1_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, OwnOnly: true, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<Group> expectedEntities = await (
                from @group in context.Groups
                join userGroupMembership in context.UserGroupMemberships
                    on @group.Id equals userGroupMembership.GroupId
                where userGroupMembership.UserId == user.Id
                orderby @group.Name
                select @group
            ).ToListAsync();

            IList<GroupResponse> expectedGroups = _mapperProvider.Mapper.Map<IList<GroupResponse>>(expectedEntities);
            GroupsResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedGroups, actualResponse.Groups);
            Assert.True(actualResponse.Filtering);
        }

        [Fact]
        public async Task Handle_ByStudent_ReturnsOwnOnly()
        {
            const string USER_NAME = "User";
            const string USER_ID = "4804ea03-5f0e-446c-8f25-4f21cbbe691c";

            const string GROUP_1_NAME = "Group 1";
            const string GROUP_1_ID = "e7202e06-9ff0-4c62-bc69-74178526caf0";
            const string GROUP_2_NAME = "Group 2";
            const string GROUP_2_ID = "ba243559-03dc-4b06-a5a4-c737bd45b453";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_1_ID, GROUP_1_NAME),
                new Group(GROUP_2_ID, GROUP_2_NAME),
                new UserGroupMembership(USER_ID, GROUP_1_ID)
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, OwnOnly: false, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<Group> expectedEntities = await (
                from @group in context.Groups
                join userGroupMembership in context.UserGroupMemberships
                    on @group.Id equals userGroupMembership.GroupId
                where userGroupMembership.UserId == user.Id
                orderby @group.Name
                select @group
            ).ToListAsync();

            IList<GroupResponse> expectedGroups = _mapperProvider.Mapper.Map<IList<GroupResponse>>(expectedEntities);
            GroupsResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedGroups, actualResponse.Groups);
            Assert.False(actualResponse.Filtering);
        }
        
        [Fact]
        public async Task Handle_FilterByName_ReturnsMatching()
        {
            const string USER_NAME = "User";
            const string USER_ID = "5cf0f1c7-d7ff-4c92-aa78-fd757c7997c9";

            const string GROUP_1_NAME = "Group 1 matching";
            const string GROUP_1_ID = "5114c3d0-be3c-4740-bcae-65828d5dab49";
            const string GROUP_2_NAME = "Group 2 matching";
            const string GROUP_2_ID = "bfdcd24a-a1f6-401f-a4ec-f28c955e1b34";
            const string GROUP_3_NAME = "Group 3";
            const string GROUP_3_ID = "14643106-4007-4d31-9636-0d0c99ce42f3";

            const string TERM_TO_MATCH = "matching";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER),
                new Group(GROUP_1_ID, GROUP_1_NAME),
                new Group(GROUP_2_ID, GROUP_2_NAME),
                new Group(GROUP_3_ID, GROUP_3_NAME)
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: TERM_TO_MATCH, OwnOnly: false, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<Group> expectedEntities = await context.Groups
                .Where(g => g.Name.Contains(TERM_TO_MATCH))
                .OrderBy(g => g.Name)
                .ToListAsync();

            IList<GroupResponse> expectedGroups = _mapperProvider.Mapper.Map<IList<GroupResponse>>(expectedEntities);
            GroupsResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedGroups, actualResponse.Groups);
            Assert.True(actualResponse.Filtering);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(0, 2)]
        [InlineData(2, 2)]
        public async Task Handle_SkipAndTake_ReturnsCorrectSlice(int skip, int take)
        {
            const string USER_NAME = "User";
            const string USER_ID = "035e3cff-9df9-4f3c-947f-02d5b82c1bc7";

            const string GROUP_1_NAME = "Group 1";
            const string GROUP_1_ID = "00ef74e6-7a7b-4764-a551-715d0833f576";
            const string GROUP_2_NAME = "Group 2";
            const string GROUP_2_ID = "27c6252f-c70d-49e0-88f2-52decb32c3d4";
            const string GROUP_3_NAME = "Group 3";
            const string GROUP_3_ID = "37017cdd-b0f0-4a4f-af6e-9282710b95f5";
            const string GROUP_4_NAME = "Group 4";
            const string GROUP_4_ID = "5e466192-c76d-4f2e-ab91-5b874d0411bc";
            const string GROUP_5_NAME = "Group 5";
            const string GROUP_5_ID = "d4993ee0-8a52-4971-afed-6db68a901377";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER),
                new Group(GROUP_1_ID, GROUP_1_NAME),
                new Group(GROUP_2_ID, GROUP_2_NAME),
                new Group(GROUP_3_ID, GROUP_3_NAME),
                new Group(GROUP_4_ID, GROUP_4_NAME),
                new Group(GROUP_5_ID, GROUP_5_NAME)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, OwnOnly: false, skip, take);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<Group> expectedEntities = await context.Groups
                .OrderBy(g => g.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            IList<GroupResponse> expectedGroups = _mapperProvider.Mapper.Map<IList<GroupResponse>>(expectedEntities);
            GroupsResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedGroups, actualResponse.Groups);
            Assert.False(actualResponse.Filtering);
            Assert.Equal(skip, actualResponse.Skipping);
            Assert.Equal(take, actualResponse.Taking);
        }
    }
}
