using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public static class UserSynchronizationExtensions
    {
        public static IServiceCollection AddUserSynchronization(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserSynchronizationService>();

            return serviceCollection;
        }

        public static IApplicationBuilder UseUserSynchronization(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<UserSynchronizationMiddleware>();

            return builder;
        }
    }
}
