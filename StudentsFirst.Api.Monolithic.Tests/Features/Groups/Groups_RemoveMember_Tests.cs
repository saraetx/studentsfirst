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
    public class Groups_RemoveMember_Tests : IDisposable
    {
        public Groups_RemoveMember_Tests()
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
        public async Task Handle_ExistingMember_RemovesMember()
        {
            const string USER_NAME = "User";
            const string USER_ID = "73578a43-ca4e-4cd5-b3d5-22ca62227463";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "10fe0fa3-4d7a-47af-83b5-1eda1a27c91f";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;
            
            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            RemoveMember.Request request = new RemoveMember.Request(GROUP_ID, USER_ID);
            RemoveMember.Handler handler = new RemoveMember.Handler(providedContext, _mapperProvider.Mapper);

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);

            await handler.Handle(request);

            UserGroupMembership? userGroupMembership = await context.UserGroupMemberships.SingleOrDefaultAsync();
            
            Assert.Null(userGroupMembership);
        }

        [Fact]
        public async Task Handle_InexistentMember_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "7137dbad-4a21-4840-bf25-12bd12555ec4";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "213389a6-8026-48e3-8582-7b952ce18b9b";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            RemoveMember.Request request = new RemoveMember.Request(GROUP_ID, USER_ID);
            RemoveMember.Handler handler = new RemoveMember.Handler(providedContext, _mapperProvider.Mapper);

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_InexistentUserId_ThrowsNotFound()
        {
            const string INEXISTENT_USER_ID = "00000000-0000-0000-0000-000000000000";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "573408ec-aa62-4350-835c-e6546ea65499";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new Group(GROUP_ID, GROUP_NAME)
            });

            await context.SaveChangesAsync();

            RemoveMember.Request request = new RemoveMember.Request(GROUP_ID, INEXISTENT_USER_ID);
            RemoveMember.Handler handler = new RemoveMember.Handler(providedContext, _mapperProvider.Mapper);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }

        [Fact]
        public async Task Handle_InexistentGroupId_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "2dc1c147-2c3c-4ea0-9a90-8a9573fa9716";

            const string INEXISTENT_GROUP_ID = "00000000-0000-0000-0000-000000000000";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            RemoveMember.Request request = new RemoveMember.Request(INEXISTENT_GROUP_ID, USER_ID);
            RemoveMember.Handler handler = new RemoveMember.Handler(providedContext, _mapperProvider.Mapper);

            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }
    }
}
