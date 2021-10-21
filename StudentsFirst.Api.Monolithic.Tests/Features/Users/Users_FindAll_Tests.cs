using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsFirst.Api.Monolithic.Features.Users;
using StudentsFirst.Api.Monolithic.Infrastructure;
using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Constants;
using StudentsFirst.Common.Dtos.Users;
using StudentsFirst.Common.Models;
using Xunit;

namespace StudentsFirst.Api.Monolithic.Tests.Features.Users
{
    public class Users_FindAll_Tests : IDisposable
    {
        public Users_FindAll_Tests()
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
            const string USER_1_ID = "b5651b01-7c89-40fb-adba-2c7e34363153";
            const string USER_2_NAME = "a. User 2";
            const string USER_2_ID = "0944d0f7-50f8-4805-909d-36ce84fcf1e6";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.TEACHER),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, Role: null, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<User> expectedEntities = await context.Users.OrderBy(u => u.Name).ToListAsync();

            IList<UserResponse> expectedUsers = _mapperProvider.Mapper.Map<IList<UserResponse>>(expectedEntities);
            UsersResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedUsers, actualResponse.Users);
            Assert.False(actualResponse.Filtering);
        }

        [Fact]
        public async Task Handle_ByStudents_ReturnsSelfAndTeachersAndStudentsSharingGroupOnly()
        {
            const string USER_1_NAME = "User 1";
            const string USER_1_ID = "3fed6212-6172-4c5a-9457-90a3505ca92a";
            const string USER_2_NAME = "User 2";
            const string USER_2_ID = "835c455d-d74d-4266-8661-52595243c321";
            const string USER_3_NAME = "User 3";
            const string USER_3_ID = "870b9cda-637e-4ac6-831f-08fc28d11837";
            const string USER_4_NAME = "User 4";
            const string USER_4_ID = "a22f6234-d3c9-48f1-b8bb-581679595aac";

            const string GROUP_NAME = "Group";
            const string GROUP_ID = "8b6c9b1d-a2fb-44c1-8233-399c774311aa";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.STUDENT),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.STUDENT),
                new User(USER_3_ID, USER_3_NAME, RoleConstants.STUDENT),
                new User(USER_4_ID, USER_4_NAME, RoleConstants.TEACHER),
                new Group(GROUP_ID, GROUP_NAME),
                new UserGroupMembership(USER_1_ID, GROUP_ID),
                new UserGroupMembership(USER_2_ID, GROUP_ID)
            });

            await context.SaveChangesAsync();

            User user = await context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, Role: null, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<User> expectedEntities = await context.Users
                .Where(u => u.Role == RoleConstants.TEACHER)
                .Concat(context.Users.Where(u => u.Id == USER_1_ID || u.Id == USER_2_ID))
                .OrderBy(u => u.Name)
                .ToListAsync();

            IList<UserResponse> expectedUsers = _mapperProvider.Mapper.Map<IList<UserResponse>>(expectedEntities);
            UsersResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedUsers, actualResponse.Users);
        }

        [Fact]
        public async Task Handle_FilterByName_ReturnsMatching()
        {
            const string USER_1_NAME = "User 1 matching";
            const string USER_1_ID = "6d8c52ef-ce92-4833-ba72-8d47baca28be";
            const string USER_2_NAME = "User 2 matching";
            const string USER_2_ID = "280e7a4c-b941-4836-9aeb-c6d3c21fd720";
            const string USER_3_NAME = "User 3";
            const string USER_3_ID = "309c98d3-8e51-4c99-a875-d8d8ffbf7fb1";

            const string TERM_TO_MATCH = "matching";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.TEACHER),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.TEACHER),
                new User(USER_3_ID, USER_3_NAME, RoleConstants.TEACHER),
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: TERM_TO_MATCH, Role: null, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<User> expectedEntities = await context.Users
                .Where(u => u.Name.Contains(TERM_TO_MATCH))
                .OrderBy(u => u.Name)
                .ToListAsync();

            IList<UserResponse> expectedUsers = _mapperProvider.Mapper.Map<IList<UserResponse>>(expectedEntities);
            UsersResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedUsers, actualResponse.Users);
            Assert.True(actualResponse.Filtering);
        }

        [Theory]
        [InlineData(RoleConstants.STUDENT)]
        [InlineData(RoleConstants.TEACHER)]
        public async Task Handle_FilterByRole_ReturnsMatching(string role)
        {
            const string USER_1_NAME = "User 1";
            const string USER_1_ID = "901ea916-a7b4-4f8c-9bdc-48e36d051093";
            const string USER_2_NAME = "User 2";
            const string USER_2_ID = "8c6e5dc8-fd0b-443c-a41d-13442e3372a0";

            using StudentsFirstContext context = _dbContextProvider.Context;
            using StudentsFirstContext providedContext = _dbContextProvider.Context;

            context.AddRange(new object[]
            {
                new User(USER_1_ID, USER_1_NAME, RoleConstants.STUDENT),
                new User(USER_2_ID, USER_2_NAME, RoleConstants.TEACHER)
            });

            await context.SaveChangesAsync();

            User user = await _dbContextProvider.Context.Users.SingleAsync(u => u.Id == USER_1_ID);
            IUserAccessorService userAccessorService = new UserAccessorService(user);

            FindAll.Request request = new FindAll.Request(NameIncludes: null, role, Skip: 0, Take: 100);
            FindAll.Handler handler = new FindAll.Handler(providedContext, _mapperProvider.Mapper, userAccessorService);

            IList<User> expectedEntities = await context.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.Name)
                .ToListAsync();

            IList<UserResponse> expectedUsers = _mapperProvider.Mapper.Map<IList<UserResponse>>(expectedEntities);
            UsersResponse actualResponse = await handler.Handle(request);

            Assert.Equal(expectedUsers, actualResponse.Users);
            Assert.True(actualResponse.Filtering);
        }
    }
}
