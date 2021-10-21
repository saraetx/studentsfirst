using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public interface IUserAccessorService
    {
        public User? User { get; }
    }
}
