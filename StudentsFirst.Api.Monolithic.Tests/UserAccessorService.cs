using StudentsFirst.Api.Monolithic.Infrastructure.Auth;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Tests
{
    public class UserAccessorService : IUserAccessorService
    {
        public UserAccessorService(User? user)
        {
            User = user;
        }

        public User? User { get; }
    }
}
