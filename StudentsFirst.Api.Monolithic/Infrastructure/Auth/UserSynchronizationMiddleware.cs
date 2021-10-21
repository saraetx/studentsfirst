using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsFirst.Api.Monolithic.Errors;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public class UserSynchronizationMiddleware
    {
        public UserSynchronizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext context, UserSynchronizationService userSynchronizationService)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                try
                {
                    var user = await userSynchronizationService.SynchronizeUser(context.User);
                    context.Items["ApplicationUser"] = user;
                }
                catch (ClaimsInvalidException cie)
                {
                    throw new RestException(HttpStatusCode.Forbidden, new ProblemDetails()
                    {
                        Title = "Claims invalid.",
                        Detail = cie.Message
                    });
                }
            }

            await _next(context);
        }
    }
}
