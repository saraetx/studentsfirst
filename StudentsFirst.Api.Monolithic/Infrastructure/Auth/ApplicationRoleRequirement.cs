using Microsoft.AspNetCore.Authorization;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public class ApplicationRoleRequirement : IAuthorizationRequirement
    {
        public ApplicationRoleRequirement(string role)
        {
            Role = role;
        }

        public string Role { get; }
    }
}
