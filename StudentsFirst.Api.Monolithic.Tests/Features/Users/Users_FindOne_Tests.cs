using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Errors;
using StudentsFirst.Api.Monolithic.Features.Users;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;
using Xunit;

namespace StudentsFirst.Api.Monolithic.Tests.Features.Users
{
    public class Users_FindOne_Tests : IDisposable
    {
        public Users_FindOne_Tests()
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
            const string USER_ID = "63d99643-a6c7-40eb-b7a1-5f30581936ac";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(USER_ID);
            FindOne.Handler handler = new FindOne.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
        
            User expectedEntity = await context.Users.SingleAsync(u => u.Id == USER_ID);

            UserResponse expectedResponse = _mapperProvider.Mapper.Map<UserResponse>(expectedEntity);
            UserResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task Handle_ByStudentForSelf_ReturnsMatch()
        {
            const string USER_NAME = "User";
            const string USER_ID = "06abf985-f3d9-4ee9-8ef3-d675f7c5af35";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.STUDENT)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(USER_ID);
            FindOne.Handler handler = new FindOne.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
        
            User expectedEntity = await context.Users.SingleAsync(u => u.Id == USER_ID);

            UserResponse expectedResponse = _mapperProvider.Mapper.Map<UserResponse>(expectedEntity);
            UserResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task Handle_ByStudentForTeacher_ReturnsMatch()
        {
            const string USER_1_NAME = "User 1";
            const string USER_1_ID = "b3027b17-3e04-4a9c-9bb6-eae6116bd0cf";
            const string USER_2_NAME = "User 2";
            const string USER_2_ID = "26376b1e-e0ed-4ada-a7ae-e57a7f71070b";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.STUDENT),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(USER_2_ID);
            FindOne.Handler handler = new FindOne.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
        
            User expectedEntity = await context.Users.SingleAsync(u => u.Id == USER_2_ID);

            UserResponse expectedResponse = _mapperProvider.Mapper.Map<UserResponse>(expectedEntity);
            UserResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task Handle_ByStudentForStudentWithSharedGroup_ReturnsMatch()
        {
            const string USER_1_NAME = "User 1";
            const string USER_1_ID = "2ff392eb-44cc-4783-87a8-913bb4dcc7e1";
            const string USER_2_NAME = "User 2";
            const string USER_2_ID = "10327ed5-d9c5-487d-96e6-505e2d3afca8";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "6b3cb0e7-0460-446b-9416-970492eaa51b";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.STUDENT),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.STUDENT),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_1_ID, GROUP_ID),
                new UserGroupMembership(USER_2_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(USER_2_ID);
            FindOne.Handler handler = new FindOne.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
        
            User expectedEntity = await context.Users.SingleAsync(u => u.Id == USER_2_ID);

            UserResponse expectedResponse = _mapperProvider.Mapper.Map<UserResponse>(expectedEntity);
            UserResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task Handle_Inexistent_ThrowsNotFound()
        {
            const string USER_NAME = "User";
            const string USER_ID = "c8869a99-be75-4105-9365-410a31141c50";

            const string INEXISTENT_USER_ID = "00000000-0000-0000-0000-000000000000";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_ID, USER_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(INEXISTENT_USER_ID);
            FindOne.Handler handler = new FindOne.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
        
            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }        

        [Fact]
        public async Task Handle_ByStudentForStudentNoSharedGroups_ThrowsNotFound()
        {
            const string USER_1_NAME = "User 1";
            const string USER_1_ID = "418a9678-f9ff-4b38-9cb5-e2f1cf05ed5e";
            const string USER_2_NAME = "User 2";
            const string USER_2_ID = "fadb36ec-617d-4c6f-a9e7-06caca10c66b";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.STUDENT),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.STUDENT)
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindOne.Request request = new FindOne.Request(USER_2_ID);
            FindOne.Handler handler = new FindOne.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);
        
            await Assert.ThrowsAsync<NotFoundRestException>(async () => await handler.Handle(request));
        }
    }
}
