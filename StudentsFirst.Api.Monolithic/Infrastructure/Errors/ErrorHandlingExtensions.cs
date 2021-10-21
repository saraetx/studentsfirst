using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Errors
{
    public static class ErrorHandlingExtensions
    {
        public static IServiceCollection AddApplicationErrorHandling(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            return serviceCollection;
        }

        public static IApplicationBuilder UseApplicationErrorHandling(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();

            return builder;
        }
    }
}
