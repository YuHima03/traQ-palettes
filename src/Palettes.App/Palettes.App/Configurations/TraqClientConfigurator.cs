using Microsoft.Extensions.Options;

namespace Palettes.App.Configurations
{
    static class TraqClientConfigurator
    {
        public const string DefaultHttpClientName = "traq";
        public const string AuthenticatedHttpClientName = "traq.authenticated";

        public static IHttpClientBuilder AddDefaultTraqHttpClient(this IServiceCollection services)
        {
            return services
                .AddHttpClient(DefaultHttpClientName, (sp, c) =>
                {
                    var traqOptions = sp.GetService<IOptions<TraqClientOptions>>()?.Value;
                    if (traqOptions is not null)
                    {
                        c.BaseAddress = new(traqOptions.ApiBaseAddress!);
                    }
                })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    UseCookies = false,
                });
        }

        public static IHttpClientBuilder AddAuthenticatedTraqHttpClient(this IServiceCollection services)
        {
            services.AddScoped<Services.AuthenticatedTraqHttpClientFactory>();
            return services
                .AddHttpClient(AuthenticatedHttpClientName, (sp, c) =>
                {
                    var traqOptions = sp.GetService<IOptions<TraqClientOptions>>()?.Value;
                    if (traqOptions is not null)
                    {
                        c.BaseAddress = new(traqOptions.ApiBaseAddress!);
                    }
                })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    UseCookies = true,
                });
        }
    }
}
