using Default.Business.Interfaces;
using Default.Business.Interfaces.Repositories;
using Default.Business.Interfaces.Services;
using Default.Business.Notifications;
using Default.Business.Services;
using Default.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Default.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            //service
            services.AddScoped<IUserService, UserService>();

            //repository
            services.AddScoped<IUserRepository, UserRepository>();

            //outros
            services.AddScoped<INotifier, Notifier>();

            return services;
        }
    }
}
