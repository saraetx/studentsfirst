using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudentsFirst.Api.Monolithic.Errors;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Errors
{
    public class ErrorHandlingMiddleware
    {
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RestException re)
            {
                context.Response.StatusCode = (int)re.Code;
                await context.Response.WriteAsJsonAsync(re.Details as object);
            }
        }
    }
}
