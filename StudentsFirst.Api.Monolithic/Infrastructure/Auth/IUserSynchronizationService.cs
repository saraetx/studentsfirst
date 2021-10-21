using System.Security.Claims;
using System.Threading.Tasks;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public interface IUserSynchronizationService
    {
        public Task<User> SynchronizeUser(ClaimsPrincipal userPrincipal);
    }
}
