using Microsoft.Extensions.DependencyInjection;

namespace StudentsFirst.Api.Monolithic.Infrastructure.Auth
{
    public static class UserAccessorExtensions
    {
        public static IServiceCollection AddUserAccessor(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserAccessorService, UserAccessorService>();

            return serviceCollection;
        }
    }
}
