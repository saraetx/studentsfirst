using Microsoft.AspNetCore.Http;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public class UserAccessorService : IUserAccessorService
    {
        public UserAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public User? User => _httpContextAccessor.HttpContext?.Items["ApplicationUser"] as User;
    }
}
