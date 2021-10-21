using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using StudentsFirst.Common.Models;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    // Not currently used (we use role-based authorization instead).
    public class ApplicationRoleHandler : AuthorizationHandler<ApplicationRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationRoleRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                User? user = httpContext.Items["ApplicationUser"] as User;

                if (string.Equals(user?.Role, requirement.Role, StringComparison.InvariantCulture))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
