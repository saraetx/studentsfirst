using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Features.Groups;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Groups;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;
using Xunit;

namespace StudentsFirst.Api.Monolithic.Tests.Features.Groups
{
    public class Groups_ListMembers_Tests : IDisposable
    {
        public Groups_ListMembers_Tests()
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
            const string USER_1_NAME = "b. User 1";
            const string USER_1_ID = "9f81ac4e-1d17-47d8-a7a6-15a89b9d4943";
            const string USER_2_NAME = "a. User 2";
            const string USER_2_ID = "1c2a4b31-fcef-4eb2-a849-fecf308c0733";
            const string USER_3_NAME = "User 3";
            const string USER_3_ID = "929710bf-226d-4b05-b709-32c01dbc4d90";

            const string GROUP_NAME = "Group 1";
            const string GROUP_ID = "77828502-187d-495f-8a68-ee206720cdf3";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.TEACHER),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.STUDENT),
                new User(USER_3_ID, USER_3_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_1_ID, GROUP_ID),
                new UserGroupMembership(USER_2_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            Group group = await context.Groups.SingleAsync(g => g.Id == GROUP_ID);

            ListMembers.Request request = new ListMembers.Request(GROUP_ID, Skip: 0, Take: 100);
            ListMembers.Handler handler =
                new ListMembers.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
            
            IList<User> expectedEntities = await (
                from userGroupMembership in context.UserGroupMemberships
                where userGroupMembership.GroupId == GROUP_ID
                join member in context.Users on userGroupMembership.UserId equals member.Id
                orderby member.Name
                select member
            ).ToListAsync();

            GroupResponse expectedGroup = _mapperProvider.Mapper.Map<GroupResponse>(group);
            IList<UserResponse> expectedMembers = _mapperProvider.Mapper.Map<IList<UserResponse>>(expectedEntities);
            GroupMembersResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedGroup, actualResponse.Group);
            Assert.Equal(expectedMembers, actualResponse.Members);
            Assert.Equal(expectedMembers.Count, actualResponse.Total);
        }

        [Fact]
        public async Task Handle_InexistentGroup_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "e2e3f36e-7f22-4869-805a-c54fac6820e1";

            const string INEXISTENT_GROUP_ID = "00000000-0000-0000-0000-000000000000";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            ListMembers.Request request = new ListMembers.Request(INEXISTENT_GROUP_ID, Skip: 0, Take: 100);
            ListMembers.Handler handler =
                new ListMembers.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_ByStudentNotOwnGroup_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "ec3e323f-f2ac-4a30-9c64-311363c4c41f";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "ad409d92-db91-45e6-a103-ac9f438d958d";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            ListMembers.Request request = new ListMembers.Request(GROUP_ID, Skip: 0, Take: 100);
            ListMembers.Handler handler =
                new ListMembers.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(0, 2)]
        [InlineData(2, 2)]
        public async Task Handle_SkipAndTake_ReturnsCorrectSlice(int skip, int take)
        {
            const string GROUP_ID = "1b62085d-e90d-4243-872b-630f7d477054";
            const string GROUP_NAME = "Group";

            const string USER_1_ID = "ebca6d1d-36a1-47e3-a668-92e5ee8bb456";
            const string USER_1_NAME = "User 1";
            const string USER_2_ID = "47fdfcad-45f0-49de-b46b-5a28b506e6ec";
            const string USER_2_NAME = "User 2";
            const string USER_3_ID = "93581765-6953-412b-b481-ab9714ffce2f";
            const string USER_3_NAME = "User 3";
            const string USER_4_ID = "13eb6dd8-4fb2-4a74-ad22-9f2eb8cac727";
            const string USER_4_NAME = "User 4";
            const string USER_5_ID = "e3a81936-34f9-4d08-8d9e-f3ff3bd249c2";
            const string USER_5_NAME = "User 5";
            
            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.TEACHER),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.STUDENT),
                new User(USER_3_ID, USER_3_NAME, RoleConstants.STUDENT),
                new User(USER_4_ID, USER_4_NAME, RoleConstants.STUDENT),
                new User(USER_5_ID, USER_5_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_1_ID, GROUP_ID),
                new UserGroupMembership(USER_2_ID, GROUP_ID),
                new UserGroupMembership(USER_3_ID, GROUP_ID),
                new UserGroupMembership(USER_4_ID, GROUP_ID),
                new UserGroupMembership(USER_5_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            ListMembers.Request request = new ListMembers.Request(GROUP_ID, skip, take);
            ListMembers.Handler handler =
                new ListMembers.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<User> expectedEntities = await context.Users
                .OrderBy(u => u.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            IList<UserResponse> expectedMembers = _mapperProvider.Mapper.Map<IList<UserResponse>>(expectedEntities);
            GroupMembersResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedMembers, actualResponse.Members);
            Assert.Equal(context.Users.Count(), actualResponse.Total);
            Assert.Equal(skip, actualResponse.Skipping);
            Assert.Equal(take, actualResponse.Taking);
        }
    }
}
