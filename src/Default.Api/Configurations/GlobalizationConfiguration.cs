using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Default.Api.Configurations
{
    public static class GlobalizationConfiguration
    {
        public static IApplicationBuilder UseGlobalizationConfiguration(this IApplicationBuilder app)
        {
            var supportedCultures = new []
            {
                new CultureInfo("pt-BR"),
                new CultureInfo("en-US"),
            };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures =  supportedCultures,
            };
            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
