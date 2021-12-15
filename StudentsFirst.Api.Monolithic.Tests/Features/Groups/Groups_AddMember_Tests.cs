using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Features.Groups;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos;
using StudentsFirst.Common.Models;
using Xunit;

namespace StudentsFirst.Api.Monolithic.Tests.Features.Groups
{
    public class Groups_AddMember_Tests : IDisposable
    {
        public Groups_AddMember_Tests()
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
        public async Task Handle_NewMember_AddsMember()
        {
            const string USER_NAME = "User";
            const string USER_ID = "ad851e88-7d1f-4d6d-9ac5-d94fbfecfce3";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "7b53f98e-91d9-44b9-b081-cc138fd1150e";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;
            
            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            AddMember.Request request = new AddMember.Request(GROUP_ID, USER_ID);
            AddMember.Handler handler = new AddMember.Handler(providedContext, _mapperProvider.Mapper);

            bool actualResponse = await handler.Handle(request);

            Assert.True(actualResponse);

            UserGroupMembership? createdUserGroupMembership = await context.UserGroupMemberships
                .SingleOrDefaultAsync();
            
            Assert.NotNull(createdUserGroupMembership);
            Assert.Equal(USER_ID, createdUserGroupMembership!.UserId);
            Assert.Equal(GROUP_ID, createdUserGroupMembership.GroupId);
        }

        [Fact]
        public async Task Handle_ExistingMember_ReturnsFalse()
        {
            const string USER_NAME = "User";
            const string USER_ID = "ad851e88-7d1f-4d6d-9ac5-d94fbfecfce3";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "7b53f98e-91d9-44b9-b081-cc138fd1150e";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            AddMember.Request request = new AddMember.Request(GROUP_ID, USER_ID);
            AddMember.Handler handler = new AddMember.Handler(providedContext, _mapperProvider.Mapper);

            bool actualResponse = await handler.Handle(request);

            Assert.False(actualResponse);
        }

        [Fact]
        public async Task Handle_InexistentUserId_ThrowsNotFound()
        {
            const string INEXISTENT_USER_ID = "00000000-0000-0000-0000-000000000000";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "7b53f98e-91d9-44b9-b081-cc138fd1150e";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            AddMember.Request request = new AddMember.Request(GROUP_ID, INEXISTENT_USER_ID);
            AddMember.Handler handler = new AddMember.Handler(providedContext, _mapperProvider.Mapper);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_InexistentGroupId_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "6054ceb6-6f5a-41f5-a273-67ad4fee4d68";

            const string INEXISTENT_GROUP_ID = "00000000-0000-0000-0000-000000000000";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            AddMember.Request request = new AddMember.Request(INEXISTENT_GROUP_ID, USER_ID);
            AddMember.Handler handler = new AddMember.Handler(providedContext, _mapperProvider.Mapper);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }
    }
}
