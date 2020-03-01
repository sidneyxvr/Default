using Microsoft.Extensions.DependencyInjection;

namespace Default.Api.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection WebApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            return services;
        }
    }
}
