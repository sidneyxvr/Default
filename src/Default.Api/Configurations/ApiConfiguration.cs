using Default.Api.Extensions;
using Default.Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Default.Api.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection WebApiConfiguration(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new CustomValidationMetadataProvider(typeof(MessageValidation)));
            })
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(MessageValidation));
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            return services;
        }
    }
}
